using Remilia.RemiliaCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using Remilia.RemiliaCode.Cards.Basic;
using Remilia.RemiliaCode.Relics;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;

namespace Remilia.RemiliaCode.Character;

[RegisterCharacter]
public class Remilia : ModCharacterTemplate<RemiliaCardPool, RemiliaRelicPool, RemiliaPotionPool>
{
    public const string CharacterId = "Remilia";

    public static readonly Color Color = new("FF3030");

    public override Color NameColor => Color;
    public override Color MapDrawingColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 85;
    public override int StartingGold => 99;
    public override float AttackAnimDelay => 0.15f;
    public override float CastAnimDelay => 0.25f;

    public override List<string> GetArchitectAttackVfx() =>
    [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];

#pragma warning disable CS0618
    protected override IEnumerable<StartingDeckEntry> StartingDeckEntries =>
    [
        StartingDeckEntry.Of<StrikeRemilia>(4),
        StartingDeckEntry.Of<DefendRemilia>(4),
        StartingDeckEntry.Of<ScarletFigure>(),
        StartingDeckEntry.Of<BloodSucking>()
    ];

    protected override IEnumerable<Type> StartingRelicTypes =>
    [
        typeof(RemiliaRelicScarletBlood)
    ];
#pragma warning restore CS0618

    public override string? CustomVisualsPath => "remilia_character.tscn".CharacterUiPath();
    public override string? CustomCharacterSelectBgPath => "remilia_background.tscn".CharacterUiPath();
    public override string? CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string? CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string? CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string? CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    public override string? CustomIconPath => "character_icon_char.tscn".CharacterUiPath();
    public override string? CustomMerchantAnimPath => "character_merchant.tscn".CharacterUiPath();
    public override string? CustomRestSiteAnimPath => "remilia_rest_site.tscn".CharacterUiPath();
    public override string? CustomArmPointingTexturePath => "multiplayer_hand_remilia_point.png".CharacterUiPath();
    public override string? CustomArmRockTexturePath => "multiplayer_hand_remilia_rock.png".CharacterUiPath();
    public override string? CustomArmPaperTexturePath => "multiplayer_hand_remilia_paper.png".CharacterUiPath();
    public override string? CustomArmScissorsTexturePath => "multiplayer_hand_remilia_scissors.png".CharacterUiPath();
}
