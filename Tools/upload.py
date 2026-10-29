# -*- coding: utf-8 -*-

import os
import sys

from typing import List

from alibabacloud_umeng_apm20220214.client import Client as umeng_apm20220214Client
from alibabacloud_tea_openapi import models as open_api_models
from alibabacloud_umeng_apm20220214 import models as umeng_apm_20220214_models
from alibabacloud_tea_util import models as util_models
from alibabacloud_tea_util.client import Client as UtilClient

def exit(code, error=None):
    if error:
        print(error)
    sys.exit(code)


def create_client(
    access_key_id: str,
    access_key_secret: str,
) -> umeng_apm20220214Client:
    """
    使用AK&SK初始化账号Client
    @param access_key_id:
    @param access_key_secret:
    @return: Client
    @throws Exception
    """
    config = open_api_models.Config(
        # 必填，您的 AccessKey ID,
        access_key_id=access_key_id,
        # 必填，您的 AccessKey Secret,
        access_key_secret=access_key_secret
    )
    # 访问的域名
    config.endpoint = f'apm.openapi.umeng.com'
    return umeng_apm20220214Client(config)

def upload_file(file_path: str, upload_address: str, OSSAccessKeyId: str, key:str, policy: str, singature: str, callback: str) -> None:
    """curl --location --request POST 'https://quickbird.oss-cn-shanghai.aliyuncs.com' \
--form 'key="tmp/EXAMPLE_DIR/symbol.so"' \
--form 'policy="YOUR_OSS_POLICY_BASE64"' \
--form 'OSSAccessKeyId=YOUR_ACCESS_KEY_ID' \
--form 'success_action_status=200' \
--form 'callback="YOUR_CALLBACK_BASE64"' \
--form 'signature="YOUR_SIGNATURE"' \
--form 'file="/temp/symbol1.so"'
    """
    cmds = ["curl"]
    cmds.append("-X POST")
    cmds.append('-F "key=%s"' % key)
    cmds.append('-F "policy=%s"' % policy)
    cmds.append('-F "OSSAccessKeyId=%s"' % OSSAccessKeyId)
    cmds.append('-F "success_action_status=200"')
    cmds.append('-F "callback=%s"' % callback)
    cmds.append('-F "signature=%s"' % singature)
    cmds.append('-F "file=@%s"' % file_path)
    cmds.append(upload_address)
    cmd = " ".join(cmds)
    print("upload_file:: ", cmd)
    return os.system(cmd)
    
def main(access_key_id:str, access_key_secret:str, app_key:str, version:str, file_path:str) -> None:
    # m/document_detail/378659.html
    client = create_client(access_key_id, access_key_secret)
    (_, file_name) = os.path.split(file_path)
    get_sym_upload_param_request = umeng_apm_20220214_models.GetSymUploadParamRequest(
        app_version= version,
        data_source_id= app_key,
        file_name= file_name,
        file_type=2
    )
    headers = {}
    try:
        # 复制代码运行请自行打印 API 的返回值
        resp = client.get_sym_upload_param_with_options(get_sym_upload_param_request, headers, util_models.RuntimeOptions())
        result = resp.body
        if not result:
            return exit(1, "error reponse")
        if not result.success:
            return exit(2, "request upload params fail:" + result.code)
        data = result.data
        return upload_file(file_path, data.upload_address, data.access_key_id, data.key, data.policy, data.signature, data.callback)
    except Exception as error:
        # 如有需要，请打印 error
        UtilClient.assert_as_string(error.message)
        
        
if __name__ == '__main__':
    access_key_id = sys.argv[1]
    access_key_secret = sys.argv[2]
    app_key = sys.argv[3]
    version = sys.argv[4]
    file_path = sys.argv[5]
    main(access_key_id, access_key_secret, app_key, version, os.path.abspath(file_path))
