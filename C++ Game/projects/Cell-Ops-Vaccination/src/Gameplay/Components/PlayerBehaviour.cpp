#include "PlayerBehaviour.h"
#include <GLFW/glfw3.h>
#include "Utils/ImGuiHelper.h"
#include "Gameplay/Components/ComponentManager.h"
#include <Gameplay/Components/EnemyBehaviour.h>



PlayerBehaviour::PlayerBehaviour() :
	IComponent()
{ }

PlayerBehaviour::~PlayerBehaviour() = default;

void PlayerBehaviour::Awake()
{
	
}

void PlayerBehaviour::OnTriggerVolumeLeaving(const std::shared_ptr<Gameplay::Physics::RigidBody>& body)
{
	LOG_INFO("Player Left trigger: {}", body->GetGameObject()->Name);
}
void PlayerBehaviour::OnTriggerVolumeEntered(const std::shared_ptr<Gameplay::Physics::RigidBody>& body)
{
	LOG_INFO("Player Entered trigger: {}", body->GetGameObject()->Name);
	if (body->GetGameObject()->Name == "Enemy") {
		LOG_INFO("Enemy Take Damage");
		body->GetGameObject()->Get<EnemyBehaviour>()->TakeDamage();
	}
}
void PlayerBehaviour::RenderImGui() {
	
}

nlohmann::json PlayerBehaviour::ToJson() const {
	return {
		
	};
}


PlayerBehaviour::Sptr PlayerBehaviour::FromJson(const nlohmann::json & blob) {
	PlayerBehaviour::Sptr result = std::make_shared<PlayerBehaviour>();
	
	return result;
}


void PlayerBehaviour::Update(float deltaTime) {
	GetGameObject()->SetRotation(GetGameObject()->GetScene()->FindObjectByName("Main Camera")->GetRotation());
	GetGameObject()->SetPostion(GetGameObject()->GetScene()->FindObjectByName("Main Camera")->GetPosition());
}


