using System;
using MelonLoader;
using UnityEngine;
using Steamworks; 

[assembly: MelonInfo(typeof(PeakClient.Load), "PEAK Ultimate Unlocker", "1.0", "G4T0XX")]
[assembly: MelonGame("", "")] 

namespace PeakClient
{
    public class Load : MelonMod
    {
        public static GameObject PeakModGO;

        public override void OnInitializeMelon()
        {
            // Cria o objeto invisível no jogo
            PeakModGO = new GameObject("PeakUnlockerMenu");
            UnityEngine.Object.DontDestroyOnLoad(PeakModGO);
            PeakModGO.hideFlags |= HideFlags.HideAndDontSave;

            PeakModGO.AddComponent<ClientMain>();
            MelonLogger.Msg("Menu do PEAK carregado! Pressione INSERT no jogo.");
        }
    }

    public class ClientMain : MonoBehaviour
    {
        private bool _showMenu = true;
        private Rect _windowRect = new Rect(20, 20, 350, 180);

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                _showMenu = !_showMenu;
            }
        }

        public void OnGUI()
        {
            if (!_showMenu) return;

            GUI.backgroundColor = Color.grey;
            _windowRect = GUI.Window(0, _windowRect, DrawMenu, "PEAK UNLOCKER | G4T0XX");
        }

        private void DrawMenu(int windowID)
        {
            GUILayout.Space(10);
            GUILayout.Label("Pressione [INSERT] para ocultar o menu.");
            GUILayout.Space(20);

            if (GUILayout.Button("DESBLOQUEAR 100% DAS CONQUISTAS (PEAK)", GUILayout.Height(50)))
            {
                UnlockAll();
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 20)); // Permite arrastar o menu
        }

        private void UnlockAll()
        {
            // Array com todas as Badges extraídas da sua lista
            string[] achievements = {
                "BeachcomberBadge", "EsotericaBadge", "TrailblazerBadge", "HappyCamperBadge",
                "VolcanologyBadge", "AscenderBadge", "AlpinistBadge", "FirstAidBadge",
                "PlundererBadge", "CookingBadge", "ForagingBadge", "NomadBadge",
                "ArboristBadge", "PeakBadge", "EmergencyPreparednessBadge", "TriedYourBestBadge",
                "EnduranceBadge", "ForestryBadge", "MentorshipBadge", "WebSecurityBadge",
                "ClutchBadge", "SurvivalistBadge", "BingBongBadge", "AnimalSerenadingBadge",
                "ToxicologyBadge", "TreadLightlyBadge", "CoolCucumberBadge", "BoulderingBadge",
                "LoneWolfBadge", "UndeadEncounterBadge", "DaredevilBadge", "SpeedClimberBadge",
                "BookwormBadge", "MycologyBadge", "AeronauticsBadge", "NaturalistBadge",
                "CalciumIntakeBadge", "AstronomyBadge", "BalloonBadge", "MycoacrobaticsBadge",
                "UltimateBadge", "MegaentomologyBadge", "ResourcefulnessBadge", "BundledUpBadge",
                "LeaveNoTraceBadge", "GourmandBadge", "NeedlepointBadge", "TwentyFourKaratBadge",
                "AdvancedMycologyBadge", "KnotTyingBadge", "CompetitiveEatingBadge",
                "DisasterResponseBadge", "CryptogastronomyBadge", "AppliedEsotericaBadge"
            };

            try
            {
                // Envia cada uma para a Steam
                foreach (string ach in achievements)
                {
                    SteamUserStats.SetAchievement(ach);
                }

                // Salva no banco de dados
                bool success = SteamUserStats.StoreStats();
                
                if (success) 
                {
                    MelonLogger.Msg("Sucesso! 100% das Badges foram ativadas.");
                    MelonLogger.Warning("Reinicie o jogo para que as conquistas apareçam no perfil da Steam.");
                }
                else
                {
                    MelonLogger.Error("Falha ao sincronizar com a Steam. Tem certeza que o jogo está conectado?");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error("Erro ao tentar desbloquear conquistas: " + ex.Message);
            }
        }
    }
}