#pragma once
#include "IComponent.h"
#include "Gameplay/GameObject.h"
#include "Gameplay/Scene.h"
#include "Gameplay/Physics/TriggerVolume.h"

struct GLFWwindow;

/// <summary>
/// hopefully this moves 
/// </summary>
class PlayerBehaviour :public Gameplay::IComponent
{
public:
	typedef std::shared_ptr<PlayerBehaviour> Sptr;

	PlayerBehaviour();
	virtual ~PlayerBehaviour();

	virtual void OnEnteredTrigger(const std::shared_ptr<Gameplay::Physics::TriggerVolume>& trigger) override;
	virtual void Awake() override;
	virtual void Update(float deltaTime) override;

public:
	virtual void RenderImGui() override;
	MAKE_TYPENAME(PlayerBehaviour);
	virtual nlohmann::json ToJson() const override;
	static PlayerBehaviour::Sptr FromJson(const nlohmann::json& blob);

protected:
	GLFWwindow* _window;

	//Gameplay::Physics::RigidBody::Sptr _body;
};