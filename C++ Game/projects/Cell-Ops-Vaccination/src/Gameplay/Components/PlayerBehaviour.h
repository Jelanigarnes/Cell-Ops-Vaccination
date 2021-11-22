#pragma once
#include "IComponent.h"
#include "Gameplay/GameObject.h"
#include "Gameplay/Scene.h"
#include "Gameplay/Physics/TriggerVolume.h"

/// <summary>
/// Player Behaviour Class
/// </summary>
class PlayerBehaviour :public Gameplay::IComponent
{
public:
	typedef std::shared_ptr<PlayerBehaviour> Sptr;

	PlayerBehaviour();
	virtual ~PlayerBehaviour();

	virtual void Awake() override;
	virtual void Update(float deltaTime) override;
	virtual void OnEnteredTrigger(const std::shared_ptr<Gameplay::Physics::TriggerVolume>& trigger) override;
	virtual void OnLeavingTrigger(const std::shared_ptr<Gameplay::Physics::TriggerVolume>& trigger) override;
	virtual void RenderImGui() override;
	virtual nlohmann::json ToJson() const override;
	static PlayerBehaviour::Sptr FromJson(const nlohmann::json& blob);
	MAKE_TYPENAME(PlayerBehaviour);

protected:
	
};