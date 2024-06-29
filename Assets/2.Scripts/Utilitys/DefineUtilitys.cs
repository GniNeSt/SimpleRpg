namespace DefineUtilitys
{
    public struct StatInformation
    {
        public int _charAtt;
        public int _charDef;
        public int _charHP;
        public int _charAvd;
        public int _charHit;

        public StatInformation(int a, int d, int h, int v, int t)
        {
            _charAtt = a;
            _charDef = d;
            _charHP = h;
            _charAvd = v;
            _charHit = t;
        }
    }
    public struct AvatarInformatin
    {
        public string _charName;
        public int _charLevel;
        public int _nowXp;
        public StatInformation _stat;

        public AvatarInformatin(string n, int l, int a, int d, int h, int v, int t, int x)
        {
            _charName = n;
            _charLevel = l;
            _stat = new StatInformation(a, d, h, v, t);
            _nowXp = x;
        }
    }
}
