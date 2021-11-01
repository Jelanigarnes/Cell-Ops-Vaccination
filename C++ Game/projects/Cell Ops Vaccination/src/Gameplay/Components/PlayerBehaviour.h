#pragma once
#include "IComponent.h"
#include "Gameplay/Physics/RigidBody.h"

/// <summary>
/// hopefully this moves 
/// </summary>
class PlayerBehaviour :public Gameplay::IComponent
{
public:
	typedef std::shared_ptr<PlayerBehaviour> Sptr;

	PlayerBehaviour();
	virtual ~PlayerBehaviour();

	virtual void Awake() override;
	virtual void Update(float deltaTime) override;

public:
	virtual void RenderImGui() override;
	MAKE_TYPENAME(PlayerBehaviour);
	virtual nlohmann::json ToJson() const override;
	static PlayerBehaviour::Sptr FromJson(const nlohmann::json& blob);

protected:
	float _speed;
	float _posX;
	float _posY;

	Gameplay::Physics::RigidBody::Sptr _body;
};