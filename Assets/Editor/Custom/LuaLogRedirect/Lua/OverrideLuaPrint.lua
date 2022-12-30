print("This is a script from a utf8 file")
print("tolua: 你好! こんにちは! 안녕하세요!")

ScriptsFromFile = {}

function ScriptsFromFile.Test1(param)
    ScriptsFromFile.Test2(param)
end

function ScriptsFromFile.Test2(param)
    print('ScriptsFromFile.Test2()', param)
    LogInfo('ScriptsFromFile.Test2()', param)
end

return ScriptsFromFile