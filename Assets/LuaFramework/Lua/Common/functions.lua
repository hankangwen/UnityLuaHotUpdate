local function ToString(...)
	local args = { ... }
	local count = select("#", ...)
	for i = 1, count, 1 do
		args[i] = tostring(args[i])
	end
	return table.concat(args, '\t')
end

local function _FormatLog(fmt, ...)
	local s = string.format(fmt, select (1, ...))
	return s
end

local function _WithTrace(...)
	local s = ToString(...)
	local tStackInfo = debug.traceback()
	s = s.."\n"..tStackInfo
	return s
end

--输出日志--
function log(...)
	GameLogger.Log(ToString(...))
end

function logFormat(fmt, ...)
	GameLogger.Log(_FormatLog(fmt, ...))
end

function logGreen(...)
	GameLogger.LogGreen(ToString(...))
end

function logGreenFormat(fmt, ...)
	GameLogger.LogGreen(_FormatLog(fmt, ...))
end

--错误日志--
function logError(...)
	GameLogger.LogError(_WithTrace(...))
end

function logErrorFormat(fmt, ...)
	local s = _FormatLog(fmt, ...)
	GameLogger.LogError(_WithTrace(s))
end

--警告日志--
function logWarn(...)
	local s = ToString(...)
	local tStackInfo = debug.traceback()
	s = s.."\n"..tStackInfo
	GameLogger.LogWarning(s);
end

function logWarningFormat(fmt, ...)
	GameLogger.LogWarning(_FormatLog(fmt, ...));
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