using Items;

public class EndFishOMeterEvent
{
    public FishItem fishItem;

    public EndFishOMeterEvent(FishItem fishItem) {
        this.fishItem = fishItem;
    }
}

public class UpdateCaptureZoneUIPositionEvent
{
    public float positionX;

    public UpdateCaptureZoneUIPositionEvent(float positionX) => this.positionX = positionX;
}

public class UpdateFishUIPositionEvent
{
    public float positionX;

    public UpdateFishUIPositionEvent(float positionX) => this.positionX = positionX;
}

public class UpdateCaptureZoneUISizeEvent
{
    public float percentage;
    public float width;

    public UpdateCaptureZoneUISizeEvent(float percentage, float width)
    {
        this.percentage = percentage;
        this.width = width;
    }
}