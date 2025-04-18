using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public Client client;
    private List<Subsystem> subsystems = [];
    private double subsystems_w = 0;
    public GameTime lastGameTime;
    public static SpriteFont arial;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        Subsystem.InitSubsystems(this);
        GLOB.spawn_map = new Map_Normal(10, 10, true, typeof(Turf_Grass));
        client = new Client(this);
        client.SetScreen(new Screen_Menu_Main(client));
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var types = Assembly.GetExecutingAssembly().GetTypes();
        var subtypes = types.Where(t => t.IsSubclassOf(typeof(Atom)));
        foreach (Type type in subtypes) {
            Atom atom = (Atom)Activator.CreateInstance(type);
            atom.LoadContent(Content);
        }

        foreach (string sprite_path in Atom.unused_load) {
            Texture2D texture = Content.Load<Texture2D>(sprite_path);
            Atom.all_textures[sprite_path] = texture;
        }

        arial = Content.Load<SpriteFont>("fonts/Arial");
    }

    protected override void Update(GameTime gameTime)
    {
        lastGameTime = gameTime;
        processSubsystems();
        client.cur_screen.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
        client.cur_screen.Draw(gameTime, Window, _spriteBatch);
        base.Draw(gameTime);
        _spriteBatch.End();
    }

    public void AddSubsystem(Subsystem subsystem) {
        subsystems.Add(subsystem);
        subsystems_w += subsystem.max_time_part;
    }

    private void processSubsystems() {
        long start_time = GLOB.getMilliseconds();
        long has_time = 10;
        foreach (var subsystem in subsystems) {
            subsystem.doTasks(GLOB.getMilliseconds() + (int) (has_time * (subsystem.max_time_part / subsystems_w)));
            has_time = 10 - (GLOB.getMilliseconds() - start_time);
        }
    }
}

