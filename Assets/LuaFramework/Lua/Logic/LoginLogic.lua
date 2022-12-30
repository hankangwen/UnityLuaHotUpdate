LoginLogic = {}
local this = LoginLogic
local json = require 'cjson'

-- 登录
function LoginLogic.Dologin(account, pwd, cb)

	pwd = Util.md5(pwd)
	-- 读数据库
	local dbStr = PlayerPrefs.GetString("ACCOUNT_PWD", "{}")
	local db = json.decode(dbStr)
	if nil == db[account] then
		cb(3, "账号未注册，请先注册")
		return
	end
	if pwd ~= db[account] then
		cb(4, "密码不正确")
		return
	end 
	cb(0, "登录成功")
end

-- 注册
function LoginLogic.DoRegist(account, pwd, cb)
	if isStringNilOrEmpty(account) then
		cb(1, "请输入账号")
		return
	end
	if isStringNilOrEmpty(pwd) then
		cb(2, "请输入密码")
		return
	end
	pwd = Util.md5(pwd)
	-- 读数据库
	local dbStr = PlayerPrefs.GetString("ACCOUNT_PWD", "{}")
	local db = json.decode(dbStr)

	if nil ~= db[account] then
		cb(3, "账号已被注册")
		return
	end
	-- 存账号信息
	db[account] = pwd
	-- 写数据库
	PlayerPrefs.SetString("ACCOUNT_PWD", json.encode(db))
	cb(0, "登录成功")
end	