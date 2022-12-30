ShowFlyTips = {}
local this = ShowFlyTips

function ShowFlyTips.Show(str)
	local uiObj = panelMgr:InstantiateUI(3)
	local uiBinder = uiObj:GetComponent("PrefabObjBinder")
	uiBinder:SetText("text", str)
	local aniEvent = uiBinder:GetObj("aniEvent")
	aniEvent.aniEvent = function(msg)
		if "finish" == msg then
			safeDestroy(uiObj)
		end
	end
end