#include "EnemyBehaviour.h"
#include <GLFW/glfw3.h>
#include "Gameplay/GameObject.h"
#include "Gameplay/Scene.h"
#include "Utils/ImGuiHelper.h"

void EnemyBehaviour::Awake()
{
	_body = GetComponent<Gameplay::Physics::RigidBody>();
	if (_body == nullptr) {
		IsEnabled = false;
	}
}
void EnemyBehaviour::RenderImGui() {
	LABEL_LEFT(ImGui::DragFloat, "posX", &_posX, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "posY", &_posY, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "Speed", &_speed, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "Health", &_health, 1.0f);
}

nlohmann::json EnemyBehaviour::ToJson() const {
	return {
		{ "posX", _posX },
		{"posY", _posY}
	};
}

EnemyBehaviour::EnemyBehaviour() :
	IComponent(),
	_posX(0.0f),
	_posY(0.0f),
	_speed(1.0f),
	_health(20.0f)
{ }

EnemyBehaviour::~EnemyBehaviour() = default;

EnemyBehaviour::Sptr EnemyBehaviour::FromJson(const nlohmann::json & blob) {
	EnemyBehaviour::Sptr result = std::make_shared<EnemyBehaviour>();
	result->_posX = blob["posX"];
	result->_posY = blob["posY"];
	result->_health = blob["Health"];
	return result;
}