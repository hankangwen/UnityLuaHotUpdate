
--local function _FormatLog(fmt, ...)
--	local s = string.format(fmt, select (1, ...))
--	return s
--end




--function LogInfoFormat(fmt, ...)
--	print(_FormatLog(fmt, ...));--Debugger:Log 不能被KGLog捕获到，临时改成print
--end
--
--function LogErrorFormat(fmt, ...)
--	local s = _FormatLog(fmt, ...)
--	local tStackInfo = debug.traceback()
--	s = s.."\n"..tStackInfo
--	Debugger:LogError(s);
--end
--
--function LogErrorWithOutTrace(...)
--	local s = ToString(...)
--	Debugger:LogError(s);
--end
--
--function LogErrorFormatWithOutTrace(fmt, ...)
--	local s = _FormatLog(fmt, ...)
--	Debugger:LogError(s);
--end
--
--function LogWarningFormat(fmt, ...)
--	Debugger:LogWarn(_FormatLog(fmt, ...));
--end

local function ToString(...)
	local args = { ... }
	local count = select("#", ...)
	for i = 1, count, 1 do
		args[i] = tostring(args[i])
	end
	return table.concat(args, '\t')
end

--输出日志--
function log(...)
	local s = ToString(...)
	local tStackInfo = debug.traceback()
	s = s.."\n"..tStackInfo
	GameLogger.Log(s);
end
--function log(str)
--	local tStackInfo = debug.traceback()
--	str = str.."\n"..tStackInfo
--    GameLogger.Log(str);
--end

--错误日志--
function logError(...)
	local s = ToString(...)
	local tStackInfo = debug.traceback()
	s = s.."\n"..tStackInfo
	GameLogger.LogError(s);
end
--function logError(str) 
--	GameLogger.LogError(str);
--end

--警告日志--
function logWarn(...)
	GameLogger.LogWarning(ToString(...));
end
--function logWarn(str) 
--	GameLogger.LogWarning(str);
--end

function logGreen(str)
	GameLogger.LogGreen(str)
end

--查找对象--
function find(str)
	return GameObject.Find(str);
end

function destroy(obj)
	GameObject.Destroy(obj);
end

function newObject(prefab)
	return GameObject.Instantiate(prefab);
end

--创建面板--
function createPanel(name)
	PanelManager:CreatePanel(name);
end

function child(str)
	return transform:FindChild(str);
end

function subGet(childNode, typeName)		
	return child(childNode):GetComponent(typeName);
end

function findPanel(str) 
	local obj = find(str);
	if obj == nil then
		error(str.." is null");
		return nil;
	end
	return obj:GetComponent("BaseLua");
end

function isNilOrNull(obj)
	return nil == obj or null == obj
end

function safeDestroy(obj) 
	if not isNilOrNull(obj) then
		GameObject.Destroy(obj)
		obj = nil
	end
end

function isStringNilOrEmpty(str)
	return nil == str or null == str or "" == str
end