@echo ----------------------------------------------
@echo     自己証明書作成用バッチファイル
@echo ----------------------------------------------
@set "TOOL_DIR=E:\Windows Kits\10\bin\10.0.18362.0\x86"
@if not exist "%TOOL_DIR%" (
    @echo ツールが存在しません。%TOOL_DIR%
    @goto ERROR_EXIT
)
@set "PATH=%TOOL_DIR%;%PATH%"

@set "WORK_DIR=D:\Work\TestService"
@if not exist %WORK_DIR% ( 
    @echo 作業フォルダ %WORK_DIR% が存在しません。
    @goto ERROR_EXIT
)
cd  /d %WORK_DIR%

@openfiles > NUL 2>&1 
@if NOT %ERRORLEVEL% EQU 0 (
    @echo 管理者権限で実行されていません。。
    goto ERROR_EXIT
)

@SET /P ANS="証明書ファイルを作成します。よろしいですか (Y/N)？"
@if /i %ANS% NEQ y if /i %ANS% NEQ Y goto ERROR_EXIT

del %WORK_DIR%\*.*
@echo;

@echo 自己証明機関を作成します。
makecert -n "CN=ServerCN1" -a sha1 -eku 1.3.6.1.5.5.7.3.3 -r -sv ServerCert1.pvk ServerCert1.cer -ss Root -sr localMachine -cy authority -b 11/06/2019 -e 12/31/2019 
@echo;

@echo ソフトウェア発行元証明書を作成します。
cert2spc ServerCert1.cer ServerCert1.spc
@echo;

@echo 個人情報交換ファイルを作成します。
pvk2pfx -pvk ServerCert1.pvk -spc ServerCert1.spc -po pswd -pfx ServerCert1.pfx -f
@echo;

:@echo 自分宛に署名証明書を発行します。これは不要？
:makecert -pe -n "CN=ServerCN2" -ss MY -a sha1 -eku 1.3.6.1.5.5.7.3.3 -iv ServerCert1.pvk -ic ServerCert1.cer
:@echo;

:ERROR_EXIT
@SET /P ANS="～～～終了～～～"
