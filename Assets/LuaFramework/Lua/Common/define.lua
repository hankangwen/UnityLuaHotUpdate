
--协议类型--
ProtocalType = {
	BINARY = 0,
	PB_LUA = 1,
	PBC = 2,
	SPROTO = 3,
}
--当前使用的协议类型--
TestProtoType = ProtocalType.BINARY

Util = LuaFramework.Util
AppConst = LuaFramework.AppConst
LuaHelper = LuaFramework.LuaHelper
ByteBuffer = LuaFramework.ByteBuffer


networkMgr = LuaHelper.GetNetManager()

WWW = UnityEngine.WWW
GameObject = UnityEngine.GameObject
PlayerPrefs = UnityEngine.PlayerPrefs

panelMgr = PanelMgr.instance