#pragma once
#include "IComponent.h"
#include "Gameplay/Physics/RigidBody.h"

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

	virtual void Awake() override;
	virtual void Update(float deltaTime) override;

public:
	virtual void RenderImGui() override;
	MAKE_TYPENAME(PlayerBehaviour);
	virtual nlohmann::json ToJson() const override;
	static PlayerBehaviour::Sptr FromJson(const nlohmann::json& blob);

protected:
	float _speed;
	float _shiftMultipler;
	float _rotZ;
	/*glm::vec2 _mouseSensitivity;
	glm::vec3 _moveSpeeds;
	glm::dvec2 _prevMousePos;
	glm::vec2 _currentRot;
	bool _isMousePressed = false;*/
	GLFWwindow* _window;

	Gameplay::Physics::RigidBody::Sptr _body;
};