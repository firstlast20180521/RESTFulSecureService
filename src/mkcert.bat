@echo ----------------------------------------------
@echo     ���ȏؖ����쐬�p�o�b�`�t�@�C��
@echo ----------------------------------------------
@set "TOOL_DIR=E:\Windows Kits\10\bin\10.0.18362.0\x86"
@if not exist "%TOOL_DIR%" (
    @echo �c�[�������݂��܂���B%TOOL_DIR%
    @goto ERROR_EXIT
)
@set "PATH=%TOOL_DIR%;%PATH%"

@set "WORK_DIR=D:\Work\TestService"
@if not exist %WORK_DIR% ( 
    @echo ��ƃt�H���_ %WORK_DIR% �����݂��܂���B
    @goto ERROR_EXIT
)
cd  /d %WORK_DIR%

@openfiles > NUL 2>&1 
@if NOT %ERRORLEVEL% EQU 0 (
    @echo �Ǘ��Ҍ����Ŏ��s����Ă��܂���B�B
    goto ERROR_EXIT
)

@SET /P ANS="�ؖ����t�@�C�����쐬���܂��B��낵���ł��� (Y/N)�H"
@if /i %ANS% NEQ y if /i %ANS% NEQ Y goto ERROR_EXIT

del %WORK_DIR%\*.*
@echo;

@echo ���ȏؖ��@�ւ��쐬���܂��B
makecert -n "CN=ServerCN1" -a sha1 -eku 1.3.6.1.5.5.7.3.3 -r -sv ServerCert1.pvk ServerCert1.cer -ss Root -sr localMachine -cy authority -b 11/06/2019 -e 12/31/2019 
@echo;

@echo �\�t�g�E�F�A���s���ؖ������쐬���܂��B
cert2spc ServerCert1.cer ServerCert1.spc
@echo;

@echo �l�������t�@�C�����쐬���܂��B
pvk2pfx -pvk ServerCert1.pvk -spc ServerCert1.spc -po pswd -pfx ServerCert1.pfx -f
@echo;

:@echo �������ɏ����ؖ����𔭍s���܂��B����͕s�v�H
:makecert -pe -n "CN=ServerCN2" -ss MY -a sha1 -eku 1.3.6.1.5.5.7.3.3 -iv ServerCert1.pvk -ic ServerCert1.cer
:@echo;

:ERROR_EXIT
@SET /P ANS="�`�`�`�I���`�`�`"
