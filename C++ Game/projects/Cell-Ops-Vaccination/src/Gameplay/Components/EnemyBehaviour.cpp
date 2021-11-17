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
	_health = _maxHealth;
}
void EnemyBehaviour::RenderImGui() {
	LABEL_LEFT(ImGui::DragFloat, "posX", &_posX, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "posY", &_posY, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "Speed", &_speed, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "Health", &_health, 1.0f);
	LABEL_LEFT(ImGui::DragFloat, "MaxHealth", &_maxHealth, 1.0f);
}

nlohmann::json EnemyBehaviour::ToJson() const {
	return {
		{ "posX", _posX },
		{"posY", _posY},
		{"speed",_speed},
		{"health",_health},
		{"maxHealth",_maxHealth},
		{"EnemyType",EnemyType}
	};
}

EnemyBehaviour::EnemyBehaviour() :
	IComponent(),
	_posX(0.0f),
	_posY(0.0f),
	_speed(0.0f),
	_health(0.0f),
	_maxHealth(0.0f),
	EnemyType(""),
	Target(nullptr)
{}

EnemyBehaviour::~EnemyBehaviour() = default;

EnemyBehaviour::Sptr EnemyBehaviour::FromJson(const nlohmann::json & blob) {
	EnemyBehaviour::Sptr result = std::make_shared<EnemyBehaviour>();
	result->_posX = blob["posX"];
	result->_posY = blob["posY"];
	result->_health = blob["Health"];
	result->_maxHealth = blob["MaxHealth"];
	result->EnemyType = blob["EnemyType"];
	return result;
}


void EnemyBehaviour::Update(float deltaTime)
{
	//GetGameObject()->LookAt();
	GetGameObject()->SetRotation(glm::vec3(0.0f, 0.0f, 0.0f));
	//GetGameObject()->SetPostion(glm::vec3(10.0f, 0.0f, 0.0));
}
// After destroying target look for new one
void EnemyBehaviour::NewTarget()
{
	// TODO: Add your implementation code here.
}
void EnemyBehaviour::OnEnteredTrigger(const std::shared_ptr<Gameplay::Physics::TriggerVolume>&trigger)
{
	LOG_INFO("Entered trigger: {}", trigger->GetGameObject()->Name);
}