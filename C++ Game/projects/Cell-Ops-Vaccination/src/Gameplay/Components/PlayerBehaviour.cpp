#include "PlayerBehaviour.h"
#include <GLFW/glfw3.h>
#include "Utils/ImGuiHelper.h"



PlayerBehaviour::PlayerBehaviour() :
	IComponent()
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

	if (glfwGetKey(GetGameObject()->GetScene()->Window, GLFW_KEY_ESCAPE) == GLFW_PRESS) {
		if (GetGameObject()->GetScene()->IsPlaying) {
			GetGameObject()->GetScene()->IsPlaying = false;
		}
		else
			GetGameObject()->GetScene()->IsPlaying = true;
	}
	GetGameObject()->SetRotation(GetGameObject()->GetScene()->FindObjectByName("Main Camera")->GetRotation());
	GetGameObject()->SetPostion(GetGameObject()->GetScene()->FindObjectByName("Main Camera")->GetPosition());
}