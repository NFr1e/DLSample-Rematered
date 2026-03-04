namespace DLSample.Shared
{
    public struct DLSampleConsts
    {
        public struct Editor
        {
            //Level
            public const string CREATE_MENU_LEVELDATA_MENU_NAME = "DLSample/Level/LevelData";
            public const string CREATE_MENU_LEVELDATA_FILE_NAME = "LevelData";
            public const int CREATE_MENU_LEVELDATA_ORDER = 1;

            public const string CREATE_MENU_BEATMAPDATA_MENU_NAME = "DLSample/Level/BeatmapData";
            public const string CREATE_MENU_BEATMAPDATA_FILE_NAME = "BeatmapData";
            public const int CREATE_MENU_BEATMAPDATA_ORDER = 2;

            public const string CREATE_MENU_PATHGRAPHERASSET_MENU_NAME = "DLSample/Level/PathGrapherAsset";
            public const string CREATE_MENU_PATHGRAPHERASSET_FILE_NAME = "PathGrapherAsset";
            public const int CREATE_MENU_PATHGRAPHERASSET_ORDER = 3;

            //Global
            public const string CREATE_MENU_SKINDATA_MENU_NAME = "DLSample/Config/SkinData";
            public const string CREATE_MENU_SKINDATA_FILE_NAME = "SkinData";
            public const int CREATE_MENU_SKINDATA_ORDER = 1;
        }
        public struct Gameplay
        {
            public const int PRIORITY_BACKTRACKABLES_HANDLER = 0;

            public const int PRIORITY_PLAYER_CONTROLLER = 1;
            public const int PRIORITY_INPUT_HANDLER = 1;
            public const int PRIORITY_STATE_HANDLER = 1;
            public const int PRIORITY_CHECKPOINT_HANDLER = 1;
            public const int PRIORITY_SOUNDTRACK_DIRECTOR = 1;
            public const int PRIORITY_TIMER_DIRECTOR = 1;

            public const int PRIORITY_SKIN_HANDLER = 2;
            public const int PRIORITY_SKIN_CHANGER = 2;

            public const int PRIORITY_INITIALIZER = 10;
        }
    }
}
