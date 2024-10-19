namespace BlueRiver.Managers
{
    public class LayerManager
    {
        private static int platformsLayer = 8;
        private static int playerLayer = 10;
        private static int pushableLayer = 25;
        private static int stairsLayer = 26;

        public static int PlatformsLayerMask = 1 << platformsLayer;
        public static int PlayerLayerMask = 1 << playerLayer;
        public static int PushableLayerMask = 1 << pushableLayer;
        public static int StairsLayerMask = 1 << stairsLayer;

        public static int ObstaclesLayerMask = LayerManager.PlatformsLayerMask | LayerManager.PushableLayerMask;
    }
}