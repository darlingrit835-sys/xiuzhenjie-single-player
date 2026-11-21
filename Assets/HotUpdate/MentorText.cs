namespace XiuZhenJie.HotUpdate
{
    public static class ClientFeatures
    {
        public static bool MeridianEntryEnabled() => false;
        public static bool MeridianAtlasEnabled() => true;
    }

    public static class MentorText
    {
        public static string Dialogue(string action)
        {
            return action switch
            {
                "ChuangGong" => "「静气凝神，接这一缕真元。」",
                "ToMai" => "「随我运息，让灵气沿经脉流转。」",
                "ChuShi" => "「此后山高水长，望你守住自己的道。」",
                _ => null
            };
        }

        public static string Caption(string action)
        {
            return action switch
            {
                "ChuangGong" => "灵气入体",
                "ToMai" => "灵流通脉",
                "ChuShi" => "落印出师",
                _ => null
            };
        }
    }
}
