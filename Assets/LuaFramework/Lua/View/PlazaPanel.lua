PlazaPanel = {}
local this = PlazaPanel

this.panelObj = nil

function PlazaPanel.Show()
	panelMgr:ShowPanel("PlazaPanel", 2)
end

function PlazaPanel.Hide()
	panelMgr:HidePanel("PlazaPanel")
end

function PlazaPanel.OnShow(obj)
	this.panelObj = obj
	local uiBinder = obj:GetComponent('PrefabObjBinder')
	-- 账号名
	uiBinder:SetText("accountText", UserData.account)
	-- 返回按钮
	uiBinder:SetBtnClick("outBtn", function() 
		this.Hide()
		LoginPanel.Show()
	end)
	-- 关注、收藏、点赞按钮
	uiBinder:SetBtnClick("likeBtn", function() 
		-- 直接打开我的CSDN博客
		UnityEngine.Application.OpenURL("https://blog.csdn.net/linxinfa")
	end)
end

function PlazaPanel.OnHide()
	this.panelObj = nil
end