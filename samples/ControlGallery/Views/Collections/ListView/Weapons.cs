namespace ControlGallery.Views.Collections.ListView;

public record Weapon(string Name, string Type, string ImageUrl);

public static class Weapons
{
    public static readonly Weapon[] Items = [
        new("Gauntlet", "Melee", "https://static.wikia.nocookie.net/quake/images/2/22/Gauntlet_v.png/revision/latest?cb=20090711033552"),
        new("Machine Gun", "Hitscan", "https://static.wikia.nocookie.net/quake/images/6/62/Mg3_g.png/revision/latest/scale-to-width-down/275?cb=20080802165101"),
        new("Shotgun", "Hitscan", "https://static.wikia.nocookie.net/quake/images/e/ee/Sg3_g.png/revision/latest/scale-to-width-down/275?cb=20080802152532"),
        new("Grenade Launcher", "Missile", "https://static.wikia.nocookie.net/quake/images/b/bf/Gl3_g.png/revision/latest?cb=20080802152241"),
        new("Rocket Launcher", "Missile", "https://static.wikia.nocookie.net/quake/images/4/43/Rl3_g.png/revision/latest/scale-to-width-down/275?cb=20080802152511"),
        new("Lightning Gun", "Beam", "https://static.wikia.nocookie.net/quake/images/d/d9/Lg3_g.png/revision/latest/scale-to-width-down/275?cb=20080802152317"),
        new("Railgun", "Hitscan", "https://static.wikia.nocookie.net/quake/images/3/34/Railgun%28Q3%29.jpg/revision/latest/scale-to-width-down/275?cb=20130424155318"),
        new("Plasma Gun", "Missile", "https://static.wikia.nocookie.net/quake/images/6/69/Plasma_Gun.jpg/revision/latest/scale-to-width-down/275?cb=20130424155100"),
        new("BFG10K", "Missile", "https://static.wikia.nocookie.net/quake/images/a/a0/Bfg3_g.png/revision/latest/scale-to-width-down/275?cb=20080802152147")
    ];
}
