namespace snek.Helpers {
  public readonly struct DifficultyLevel {
    public readonly float snakeSpeed;
    public readonly float snakeShrink;
    public readonly float foodTimer;
    public readonly int scoreThreshold;

    public DifficultyLevel(float snakeSpeed, float snakeShrink, float foodTimer, int scoreThreshold) {
      this.snakeSpeed = snakeSpeed;
      this.snakeShrink = snakeShrink;
      this.foodTimer = foodTimer;
      this.scoreThreshold = scoreThreshold;
    }
  }

}
