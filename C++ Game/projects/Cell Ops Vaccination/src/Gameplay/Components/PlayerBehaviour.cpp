#include "PlayerBehaviour.h"
#include <GLFW/glfw3.h>
#include "Gameplay/GameObject.h"
#include "Gameplay/Scene.h"
#include "Utils/ImGuiHelper.h"


PlayerBehaviour::PlayerBehaviour() :
	IComponent(),
	_speed(0.5f),
	_mouseSensitivity({ 0.5f, 0.3f }),
	_currentRot(glm::vec2(0.0f)),
	_isMousePressed(false)
{ }

void PlayerBehaviour::Awake()
{
	_window = GetGameObject()->GetScene()->Window;
	_body = GetComponent<Gameplay::Physics::RigidBody>();
	if (_body == nullptr) {
		IsEnabled = false;
	}
}

void PlayerBehaviour::RenderImGui() {
	LABEL_LEFT(ImGui::DragFloat, "Speed", &_speed, 1.0f);
	LABEL_LEFT(ImGui::DragFloat2, "Mouse Sensitivity", &_mouseSensitivity.x, 0.01f);
}

nlohmann::json PlayerBehaviour::ToJson() const {
	return {		
		{"Speed",_speed},
		{ "mouse_sensitivity", GlmToJson(_mouseSensitivity) }
	};
}

PlayerBehaviour::~PlayerBehaviour() = default;

PlayerBehaviour::Sptr PlayerBehaviour::FromJson(const nlohmann::json & blob) {
	PlayerBehaviour::Sptr result = std::make_shared<PlayerBehaviour>();
	result->_mouseSensitivity = ParseJsonVec2(blob["mouse_sensitivity"]);
	result->_speed = blob["speed"];
	return result;
}

void PlayerBehaviour::Update(float deltaTime) {
	//Player 1 Controls
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_LEFT) == GLFW_PRESS || (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_A) == GLFW_PRESS)){
		_body->ApplyImpulse(glm::vec3(-_speed, 0.0f, 0.0f));
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_RIGHT)  == GLFW_PRESS || (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_D) == GLFW_PRESS)) {
		_body->ApplyImpulse(glm::vec3(_speed, 0.0f, 0.0f));
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_UP) == GLFW_PRESS || (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_W) == GLFW_PRESS)) {
		_body->ApplyImpulse(glm::vec3(0.0f, _speed, 0.0f));
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_DOWN) == GLFW_PRESS || (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_S) == GLFW_PRESS)) {
		_body->ApplyImpulse(glm::vec3(0.0f, -_speed, 0.0f));
	}

	//Rotate with Mouse
	if (glfwGetMouseButton(_window, 0)) {
		if (_isMousePressed == false) {
			glfwGetCursorPos(_window, &_prevMousePos.x, &_prevMousePos.y);
		}
		_isMousePressed = true;
	}
	else {
		_isMousePressed = false;
	}
	if (_isMousePressed) {
		glm::dvec2 currentMousePos;
		glfwGetCursorPos(_window, &currentMousePos.x, &currentMousePos.y);
		_currentRot.x += static_cast<float>(currentMousePos.x - _prevMousePos.x) * _mouseSensitivity.x;
		_currentRot.y += static_cast<float>(currentMousePos.y - _prevMousePos.y) * _mouseSensitivity.y;
		glm::quat rotX = glm::angleAxis(glm::radians(_currentRot.x), glm::vec3(1, 0, 0));
		glm::quat rotY = glm::angleAxis(glm::radians(_currentRot.y), glm::vec3(0, 1, 0));
		glm::quat currentRot = rotX * rotY;
		//GetGameObject()->SetRotation(glm::eulerAngles(rotX,rotY));
		//GetGameObject()->LookAt(glm::vec3(currentRot,0.0f ,currentRot));
	}
}