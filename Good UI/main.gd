extends Control
#currrency 

#@onready var button_6: TextureButton = $Bottom/Characters/Button6
@onready var button_one: Button = $Bottom/Characters/ButtonONE
@onready var button_two: Button = $Bottom/Characters/ButtonTWO
@onready var button_three: Button = $Bottom/Characters/ButtonTHREE
@onready var button_four: Button = $Bottom/Characters/ButtonFOUR
@onready var button_five: Button = $Bottom/Enemy/ButtonFIVE
@onready var button_six: Button = $Bottom/Enemy/ButtonSIX

@onready var coin_number: Label = $Coins/CoinNumber
@onready var coin_number_enemy: Label = $CoinsEnemy/CoinNumberEnemy


# Called when the node enters the scene tree for the first time.
#func _ready():
#	button_1.grab_focus()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


#func _on_button_1_pressed():
	#print("Button 1!")


func _on_button_one_pressed():
	print("Button 1 !")


func _on_button_two_pressed():
	print("Button 2!")


func _on_button_three_pressed():
	print("Button 3!")


func _on_button_four_pressed():
	print("Button 4!")


func _on_button_five_pressed():
	print("Button 5!")


func _on_button_six_pressed():
	print("Button 6!")
