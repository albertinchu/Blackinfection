using Smod2;
using Smod2.Attributes;
using scp4aiur;
namespace BlackInfection
{
    [PluginDetails(
        author = "Albertinchu ",
        name = "BlackInfection",
        description = "Lo mismo que peanut infection pero 106",
        id = "albertinchu.gamemode.BlackInfection",
        version = "1.0.0",
        SmodMajor = 3,
        SmodMinor = 4,
        SmodRevision = 0
        )]
    public class BlackInfection : Plugin
    {

        public override void OnDisable()
        {
            this.Info("BlackInfection - Desactivado");
        }

        public override void OnEnable()
        {
            Info("BlackInfection - activado.");
        }

        public override void Register()
        {
            GamemodeManager.Manager.RegisterMode(this);
            Timing.Init(this);
            this.AddEventHandlers(new PlayersEvents(this));

        }
        public void RefreshConfig()
        {


        }
    }

}

