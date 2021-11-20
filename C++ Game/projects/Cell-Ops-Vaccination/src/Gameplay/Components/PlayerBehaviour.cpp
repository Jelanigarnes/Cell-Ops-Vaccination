#include "PlayerBehaviour.h"
#include <GLFW/glfw3.h>
#include "Gameplay/GameObject.h"
#include "Gameplay/Scene.h"
#include "Utils/ImGuiHelper.h"


PlayerBehaviour::PlayerBehaviour() :
	IComponent(),
	_speed(0.5f),
	_rotZ(0.0f)
	/*_mouseSensitivity({ 0.5f, 0.3f }),
	_moveSpeeds(glm::vec3(1.0f)),
	_shiftMultipler(2.0f),
	_currentRot(glm::vec2(0.0f)),
	_isMousePressed(false)*/
{ }

PlayerBehaviour::~PlayerBehaviour() = default;

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
	LABEL_LEFT(ImGui::DragFloat, "Current Rotation", &_rotZ, 1.0f);
	/*LABEL_LEFT(ImGui::DragFloat2, "Mouse Sensitivity", &_mouseSensitivity.x, 0.01f);
	LABEL_LEFT(ImGui::DragFloat3, "Move Speed       ", &_moveSpeeds.x, 0.01f, 0.01f);*/
	LABEL_LEFT(ImGui::DragFloat, "Shift Multiplier ", &_shiftMultipler, 0.01f, 1.0f);
}

nlohmann::json PlayerBehaviour::ToJson() const {
	return {
		{"Speed",_speed},
		/*{ "mouse_sensitivity", GlmToJson(_mouseSensitivity) },
		{ "move_speed", GlmToJson(_moveSpeeds) },*/
		{ "shift_mult", _shiftMultipler },
		{"Current Rotation",_rotZ}
	};
}


PlayerBehaviour::Sptr PlayerBehaviour::FromJson(const nlohmann::json & blob) {
	PlayerBehaviour::Sptr result = std::make_shared<PlayerBehaviour>();
	result->_speed = blob["speed"];
	/*result->_mouseSensitivity = ParseJsonVec2(blob["mouse_sensitivity"]);
	result->_moveSpeeds = ParseJsonVec3(blob["move_speed"]);*/
	result->_shiftMultipler = JsonGet(blob, "shift_mult", 2.0f);
	result->_rotZ = blob["Current Rotation"];
	return result;
}

void PlayerBehaviour::Update(float deltaTime) {

	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_LEFT) == GLFW_PRESS || (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_A) == GLFW_PRESS)) {
		_body->ApplyImpulse(glm::vec3(-_speed, 0.0f, 0.0f));
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_RIGHT) == GLFW_PRESS || (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_D) == GLFW_PRESS)) {
		_body->ApplyImpulse(glm::vec3(_speed, 0.0f, 0.0f));
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_UP) == GLFW_PRESS || (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_W) == GLFW_PRESS)) {
		_body->ApplyImpulse(glm::vec3(0.0f, _speed, 0.0f));
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_DOWN) == GLFW_PRESS || (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_S) == GLFW_PRESS)) {
		_body->ApplyImpulse(glm::vec3(0.0f, -_speed, 0.0f));
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_Q) == GLFW_PRESS) {
		_rotZ += 2.00f;
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_E) == GLFW_PRESS) {
		_rotZ -= 2.00f;
	}
	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_ESCAPE) == GLFW_PRESS) {
		if (GetGameObject()->GetScene()->IsPlaying) {
			GetGameObject()->GetScene()->IsPlaying = false;
		}
		else
			GetGameObject()->GetScene()->IsPlaying = true;
	}
	GetGameObject()->SetRotation(glm::vec3(90.0f, 0.0f, _rotZ));
}