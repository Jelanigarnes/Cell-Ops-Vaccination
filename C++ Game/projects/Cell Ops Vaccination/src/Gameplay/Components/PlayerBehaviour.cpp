#include "PlayerBehaviour.h"
#include <GLFW/glfw3.h>
#include "Gameplay/GameObject.h"
#include "Gameplay/Scene.h"
#include "Utils/ImGuiHelper.h"

void PlayerBehaviour::Awake()
{
	_body = GetComponent<Gameplay::Physics::RigidBody>();
	if (_body == nullptr) {
		IsEnabled = false;
	}
}

void PlayerBehaviour::RenderImGui() {
	LABEL_LEFT(ImGui::DragFloat, "posX", &_posX, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "posY", &_posY, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "Speed", &_speed, 1.0f);
}

nlohmann::json PlayerBehaviour::ToJson() const {
	return {
		{ "posX", _posX },
		{"posY", _posY}
	};
}

PlayerBehaviour::PlayerBehaviour() :
	IComponent(),
	_posX(0.0f),
	_posY(0.0f),
	_speed(1.0f)
{ }

PlayerBehaviour::~PlayerBehaviour() = default;

PlayerBehaviour::Sptr PlayerBehaviour::FromJson(const nlohmann::json & blob) {
	PlayerBehaviour::Sptr result = std::make_shared<PlayerBehaviour>();
	result->_posX = blob["posX"];
	result->_posY = blob["posY"];
	return result;
}

void PlayerBehaviour::Update(float deltaTime) {
	//Player 1 Controls
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_LEFT) == GLFW_PRESS) {
		_posX += _speed;
		/*if (_posX > -3.0f)
		{
			_posX = -3.0f;
		}*/
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_RIGHT) == GLFW_PRESS) {
		_posX -= _speed;
		/*if (_posX < -44.0f) {
			_posX = -44.0f;
		}*/
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_UP) == GLFW_PRESS) {
		_posY -= _speed;
		/*if (_posY < -44.0f) {
			_posY = -44.0f;
		}*/
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_DOWN) == GLFW_PRESS) {
		_posY += _speed;
		/*if (_posY > 44.0f) {
			_posY = 44.0f;
		}*/
	}
	GetGameObject()->SetPostion(glm::vec3(_posX, _posY, 0.0f));
}