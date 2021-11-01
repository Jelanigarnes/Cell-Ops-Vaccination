#pragma once
#include "IComponent.h"
#include "Gameplay/Physics/RigidBody.h"
class EnemyBehaviour :public Gameplay::IComponent
{
public:
	typedef std::shared_ptr<EnemyBehaviour> Sptr;

	EnemyBehaviour();
	virtual ~EnemyBehaviour();

	virtual void Awake() override;

public:
	virtual void RenderImGui() override;
	MAKE_TYPENAME(EnemyBehaviour);
	virtual nlohmann::json ToJson() const override;
	static EnemyBehaviour::Sptr FromJson(const nlohmann::json& blob);

protected:
	float _speed;
	float _posX;
	float _posY;
	float _health;
	float _dmg;

	Gameplay::Physics::RigidBody::Sptr _body;
};

