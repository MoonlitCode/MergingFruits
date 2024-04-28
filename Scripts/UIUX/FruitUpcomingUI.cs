using Godot;
using MergingFruits.Scripts.Fruits;

namespace MergingFruits.Scripts.UIUX;

public partial class FruitUpcomingUI : Control {
	[Export] private TextureRect _fruitUpcomingImage;

	private FruitInfoList _fruitInfoList;

	public void Initialize(FruitInfoList fruitInfoList) => _fruitInfoList = fruitInfoList;

	public void UpdateUpcomingImage(int index) {
		_fruitUpcomingImage.Texture = _fruitInfoList.Data[index].Texture;
	}
}