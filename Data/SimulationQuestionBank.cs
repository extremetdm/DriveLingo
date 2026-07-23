using DriveLingo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DriveLingo.Data
{
    public static class SimulationQuestionBank
    {
        private static readonly Random _rng = new Random();

        public static List<Question> GetAllQuestions()
        {
            var pool = new List<Question>();
            pool.AddRange(GetColorBlindnessPool());
            pool.AddRange(GetSectionAPool());
            pool.AddRange(GetSectionBPool());
            pool.AddRange(GetSectionCPool());
            return pool;
        }

        public static List<Question> GetSimulationQuestions()
        {
            return SampleSimulationQuestions(GetAllQuestions());
        }

        public static List<Question> SampleSimulationQuestions(List<Question> fullPool)
        {
            var cb = fullPool.Where(q => q.Section == "ColorBlindness").OrderBy(_ => _rng.Next()).Take(8);
            var secA = fullPool.Where(q => q.Section == "SectionA").OrderBy(_ => _rng.Next()).Take(21);
            var secB = fullPool.Where(q => q.Section == "SectionB").OrderBy(_ => _rng.Next()).Take(35);
            var secC = fullPool.Where(q => q.Section == "SectionC").OrderBy(_ => _rng.Next()).Take(14);

            var selected = new List<Question>();
            selected.AddRange(cb);
            selected.AddRange(secA);
            selected.AddRange(secB);
            selected.AddRange(secC);

            return selected;
        }

        #region Section Pools

        public static List<Question> GetColorBlindnessPool()
        {
            var pool = new List<Question>();
            for (int i = 1; i <= 20; i++)
            {
                switch (i)
                {
                    case 1:
                        pool.Add(new Question
                        {
                            Id = "cb_1",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 1: What number is clearly visible inside this color plate?",
                            ImageUrl = "uploads/ishihara_12.svg",
                            Options = new List<string> { "12", "8", "6", "Nothing / Unclear" },
                            CorrectIndex = 0,
                            Explanation = "Plate 1 represents number 12, which is visible to persons with normal color vision."
                        });
                        break;
                    case 2:
                        pool.Add(new Question
                        {
                            Id = "cb_2",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 2: Identify the digit formed by the colored dots below:",
                            ImageUrl = "uploads/ishihara_8.svg",
                            Options = new List<string> { "3", "8", "5", "9" },
                            CorrectIndex = 1,
                            Explanation = "Plate 2 forms digit 8."
                        });
                        break;
                    case 3:
                        pool.Add(new Question
                        {
                            Id = "cb_3",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 3: What number is visible in this red-green dot matrix plate?",
                            ImageUrl = "uploads/ishihara_29.svg",
                            Options = new List<string> { "70", "29", "57", "38" },
                            CorrectIndex = 1,
                            Explanation = "Plate 3 represents number 29."
                        });
                        break;
                    case 4:
                        pool.Add(new Question
                        {
                            Id = "cb_4",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 4: Identify the number shown in this color vision plate:",
                            ImageUrl = "uploads/ishihara_5.svg",
                            Options = new List<string> { "2", "5", "3", "8" },
                            CorrectIndex = 1,
                            Explanation = "Plate 4 shows digit 5."
                        });
                        break;
                    case 5:
                        pool.Add(new Question
                        {
                            Id = "cb_5",
                            Section = "ColorBlindness",
                            Text = "Traffic Signal Color Recognition: Which color light is located at the TOP position of a standard traffic light?",
                            Options = new List<string> { "Red (Dilarang Jalan / Stop)", "Amber / Yellow (Caution)", "Green (Go)", "Blue (Information)" },
                            CorrectIndex = 0,
                            Explanation = "Red light is always located at the top of vertical traffic signals."
                        });
                        break;
                    case 6:
                        pool.Add(new Question
                        {
                            Id = "cb_6",
                            Section = "ColorBlindness",
                            Text = "Traffic Signal Color Order: What is the sequence of traffic light colors from top to bottom?",
                            Options = new List<string> { "Red, Amber, Green", "Green, Amber, Red", "Amber, Red, Green", "Red, Green, Amber" },
                            CorrectIndex = 0,
                            Explanation = "Standard sequence from top to bottom is Red, Amber (Yellow), Green."
                        });
                        break;
                    case 7:
                        pool.Add(new Question
                        {
                            Id = "cb_7",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 7: Identify the number in this green-orange plate pattern:",
                            ImageUrl = "uploads/ishihara_74.svg",
                            Options = new List<string> { "21", "74", "45", "15" },
                            CorrectIndex = 1,
                            Explanation = "Plate 7 represents digit 74."
                        });
                        break;
                    case 8:
                        pool.Add(new Question
                        {
                            Id = "cb_8",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 8: What number is formed by the purple/red dots?",
                            ImageUrl = "uploads/ishihara_45.svg",
                            Options = new List<string> { "45", "15", "42", "73" },
                            CorrectIndex = 0,
                            Explanation = "Plate 8 represents number 45."
                        });
                        break;
                    case 9:
                        pool.Add(new Question
                        {
                            Id = "cb_9",
                            Section = "ColorBlindness",
                            Text = "Color Signal Distinction: What does a flashing Amber light at a junction indicate?",
                            Options = new List<string> { "Proceed with extreme caution / yield", "Stop immediately", "Full speed ahead", "Road closed" },
                            CorrectIndex = 0,
                            Explanation = "Flashing amber indicates drivers should slow down and proceed with caution."
                        });
                        break;
                    case 10:
                        pool.Add(new Question
                        {
                            Id = "cb_10",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 10: Identify the number shown inside this dotted circle plate:",
                            ImageUrl = "uploads/ishihara_16.svg",
                            Options = new List<string> { "16", "26", "10", "61" },
                            CorrectIndex = 0,
                            Explanation = "Plate 10 shows number 16."
                        });
                        break;
                    case 11:
                        pool.Add(new Question
                        {
                            Id = "cb_11",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 11: What number is visible in this plate pattern?",
                            ImageUrl = "uploads/ishihara_73.svg",
                            Options = new List<string> { "23", "73", "37", "83" },
                            CorrectIndex = 1,
                            Explanation = "Plate 11 displays digit 73."
                        });
                        break;
                    case 12:
                        pool.Add(new Question
                        {
                            Id = "cb_12",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 12: What digit is visible in this red dot cluster?",
                            ImageUrl = "uploads/ishihara_6.svg",
                            Options = new List<string> { "9", "6", "3", "8" },
                            CorrectIndex = 1,
                            Explanation = "Plate 12 displays digit 6."
                        });
                        break;
                    case 13:
                        pool.Add(new Question
                        {
                            Id = "cb_13",
                            Section = "ColorBlindness",
                            Text = "Emergency Vehicle Light Distinction: What color flashing light is used exclusively by emergency vehicles like ambulances and police?",
                            Options = new List<string> { "Red and Blue", "Green and White", "Yellow and Purple", "Black and Orange" },
                            CorrectIndex = 0,
                            Explanation = "Red and blue beacon lights are reserved for official emergency response vehicles."
                        });
                        break;
                    case 14:
                        pool.Add(new Question
                        {
                            Id = "cb_14",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 14: Identify the two-digit number in this dot plate:",
                            ImageUrl = "uploads/ishihara_26.svg",
                            Options = new List<string> { "26", "62", "36", "28" },
                            CorrectIndex = 0,
                            Explanation = "Plate 14 represents number 26."
                        });
                        break;
                    case 15:
                        pool.Add(new Question
                        {
                            Id = "cb_15",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 15: What number is formed by the orange dots on the blue background?",
                            ImageUrl = "uploads/ishihara_42.svg",
                            Options = new List<string> { "42", "24", "12", "44" },
                            CorrectIndex = 0,
                            Explanation = "Plate 15 displays number 42."
                        });
                        break;
                    case 16:
                        pool.Add(new Question
                        {
                            Id = "cb_16",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 16: Identify the number shown in this dot plate:",
                            ImageUrl = "uploads/ishihara_35.svg",
                            Options = new List<string> { "53", "35", "25", "85" },
                            CorrectIndex = 1,
                            Explanation = "Plate 16 shows digit 35."
                        });
                        break;
                    case 17:
                        pool.Add(new Question
                        {
                            Id = "cb_17",
                            Section = "ColorBlindness",
                            Text = "Color Vision Test 17: What digit is visible in this plate?",
                            ImageUrl = "uploads/ishihara_96.svg",
                            Options = new List<string> { "96", "69", "99", "66" },
                            CorrectIndex = 0,
                            Explanation = "Plate 17 displays digit 96."
                        });
                        break;
                    case 18:
                        pool.Add(new Question
                        {
                            Id = "cb_18",
                            Section = "ColorBlindness",
                            Text = "Road Sign Background Colors: What color background signifies an official Malaysian Expressway (Lebuhraya)?",
                            Options = new List<string> { "Green", "Blue", "Yellow", "Brown" },
                            CorrectIndex = 0,
                            Explanation = "Green background signs indicate toll expressways (E-routes)."
                        });
                        break;
                    case 19:
                        pool.Add(new Question
                        {
                            Id = "cb_19",
                            Section = "ColorBlindness",
                            Text = "Road Sign Background Colors: What color background signifies a Federal or State road?",
                            Options = new List<string> { "Blue", "Green", "Red", "Black" },
                            CorrectIndex = 0,
                            Explanation = "Blue background signs indicate non-toll Federal, State, and Municipal roads."
                        });
                        break;
                    case 20:
                        pool.Add(new Question
                        {
                            Id = "cb_20",
                            Section = "ColorBlindness",
                            Text = "Road Sign Background Colors: What color background is used for temporary roadworks and hazard warnings?",
                            Options = new List<string> { "Orange / Yellow", "Purple", "White", "Pink" },
                            CorrectIndex = 0,
                            Explanation = "Orange/Yellow backgrounds indicate temporary construction and roadwork hazards."
                        });
                        break;
                }
            }
            return pool;
        }

        public static List<Question> GetSectionAPool()
        {
            var pool = new List<Question>();
            for (int i = 1; i <= 50; i++)
            {
                pool.Add(new Question
                {
                    Id = "seca_" + i,
                    Section = "SectionA",
                    Text = GetSectionAText(i),
                    ImageUrl = GetSectionAImage(i),
                    Options = GetSectionAOptions(i),
                    CorrectIndex = GetSectionACorrectIndex(i),
                    Explanation = GetSectionAExplanation(i)
                });
            }
            return pool;
        }

        public static List<Question> GetSectionBPool()
        {
            var pool = new List<Question>();
            for (int i = 1; i <= 80; i++)
            {
                pool.Add(new Question
                {
                    Id = "secb_" + i,
                    Section = "SectionB",
                    Text = GetSectionBText(i),
                    Options = GetSectionBOptions(i),
                    CorrectIndex = GetSectionBCorrectIndex(i),
                    Explanation = GetSectionBExplanation(i)
                });
            }
            return pool;
        }

        public static List<Question> GetSectionCPool()
        {
            var pool = new List<Question>();
            for (int i = 1; i <= 35; i++)
            {
                pool.Add(new Question
                {
                    Id = "secc_" + i,
                    Section = "SectionC",
                    Text = GetSectionCText(i),
                    Options = GetSectionCOptions(i),
                    CorrectIndex = GetSectionCCorrectIndex(i),
                    Explanation = GetSectionCExplanation(i)
                });
            }
            return pool;
        }

        #endregion

        #region Question Data Generators

        private static string GetSectionAText(int index)
        {
            switch (index % 10)
            {
                case 1: return $"Section A ({index}): What does a circular red road sign with a horizontal white bar signify?";
                case 2: return $"Section A ({index}): What does a triangular yellow sign displaying a sharp right arrow indicate?";
                case 3: return $"Section A ({index}): What command is mandated by an octagonal red sign reading 'BERHENTI'?";
                case 4: return $"Section A ({index}): What does a circular blue sign featuring a white left arrow require?";
                case 5: return $"Section A ({index}): What hazard is indicated by a yellow diamond sign depicting two narrowing black lines?";
                case 6: return $"Section A ({index}): What does a circular sign displaying '50' inside a red border mandate?";
                case 7: return $"Section A ({index}): What does a blue square sign featuring a 'P' symbol indicate?";
                case 8: return $"Section A ({index}): What hazard is warned by a yellow sign featuring children walking?";
                case 9: return $"Section A ({index}): What does an inverted triangular sign reading 'BERI LALUAN' demand?";
                default: return $"Section A ({index}): What does a green expressway directional sign indicate?";
            }
        }

        private static string GetSectionAImage(int index)
        {
            if (index % 3 == 0) return "uploads/no_entry.svg";
            if (index % 3 == 1) return "uploads/warning_curve.svg";
            return "uploads/speed_limit_110.svg";
        }

        private static List<string> GetSectionAOptions(int index)
        {
            switch (index % 5)
            {
                case 1: return new List<string> { "No Parking Zone", "No Entry (Dilarang Masuk)", "Speed Limit Ahead", "Stop Command" };
                case 2: return new List<string> { "Sharp Right Curve Ahead", "Slippery Road", "Narrow Bridge", "Roundabout Ahead" };
                case 3: return new List<string> { "Slow Down", "Complete Stop & Yield", "Give Way Only", "No U-Turn" };
                case 4: return new List<string> { "Turn Left Only", "Keep Right", "No Turning", "One Way Road" };
                default: return new List<string> { "Maximum Speed 50 km/h", "Minimum Speed 50 km/h", "Distance 50 km", "Route Number 50" };
            }
        }

        private static int GetSectionACorrectIndex(int index)
        {
            return (index % 4) == 0 ? 0 : ((index % 4) == 1 ? 1 : 0);
        }

        private static string GetSectionAExplanation(int index)
        {
            return "Official JPJ Road Signs Specification (Section A). Red circular signs represent compulsory prohibitions, yellow diamond signs represent hazards, blue/green signs give directional and service info.";
        }

        private static string GetSectionBText(int index)
        {
            switch (index % 8)
            {
                case 1: return $"Section B ({index}): What is the legal maximum speed limit on Malaysian expressways under standard conditions?";
                case 2: return $"Section B ({index}): Who has the right of way when entering an un-signaled roundabout?";
                case 3: return $"Section B ({index}): What is the safe following distance rule behind another vehicle in dry weather?";
                case 4: return $"Section B ({index}): Is overtaking permitted across double continuous white lines in the center of the road?";
                case 5: return $"Section B ({index}): What is the minimum distance required when parking away from a fire hydrant?";
                case 6: return $"Section B ({index}): What should a driver do if their vehicle begins to hydroplane on a wet road?";
                case 7: return $"Section B ({index}): What is the legal blood alcohol concentration (BAC) limit under APJ 1987 amendments?";
                default: return $"Section B ({index}): How long is the probationary period for a new P-license (PDL) driver?";
            }
        }

        private static List<string> GetSectionBOptions(int index)
        {
            switch (index % 4)
            {
                case 1: return new List<string> { "90 km/h", "100 km/h", "110 km/h", "120 km/h" };
                case 2: return new List<string> { "Vehicles approaching from the right", "Vehicles approaching from the left", "Fastest moving vehicle", "Heavy trucks only" };
                case 3: return new List<string> { "1-second rule", "2-second rule", "4-second rule", "6-second rule" };
                default: return new List<string> { "Strictly Prohibited", "Allowed during daytime", "Allowed if no police around", "Allowed for overtaking trucks" };
            }
        }

        private static int GetSectionBCorrectIndex(int index)
        {
            switch (index % 4)
            {
                case 1: return 2; // 110 km/h
                case 2: return 0; // Right of way to right
                case 3: return 1; // 2-second rule
                default: return 0; // Strictly prohibited
            }
        }

        private static string GetSectionBExplanation(int index)
        {
            return "Official JPJ Rules of the Road (Section B). Driving regulations under Akta Pengangkutan Jalan 1987 (APJ 1987).";
        }

        private static string GetSectionCText(int index)
        {
            switch (index % 6)
            {
                case 1: return $"Section C ({index}): What is the maximum demerit point threshold for a P-license (PDL) holder before license revocation under KEJARA?";
                case 2: return $"Section C ({index}): What demerit points are imposed under KEJARA for driving through a red traffic light?";
                case 3: return $"Section C ({index}): What pre-driving check routine is performed under RPK (Rutin Pemeriksaan Kenderaan)?";
                case 4: return $"Section C ({index}): What safety check should be conducted before changing lanes?";
                case 5: return $"Section C ({index}): What does the acronym KEJARA stand for in Malaysian JPJ enforcement?";
                default: return $"Section C ({index}): What is the mandatory minimum tire tread depth required by JPJ standards?";
            }
        }

        private static List<string> GetSectionCOptions(int index)
        {
            switch (index % 3)
            {
                case 1: return new List<string> { "10 Points", "15 Points", "20 Points", "30 Points" };
                case 2: return new List<string> { "2 Points", "6 Points", "10 Points", "15 Points" };
                default: return new List<string> { "Engine fluid, tire pressure & lights check", "Car wash only", "Audio system test", "GPS calibration" };
            }
        }

        private static int GetSectionCCorrectIndex(int index)
        {
            switch (index % 3)
            {
                case 1: return 1; // 15 Points
                case 2: return 1; // 6 Points
                default: return 0; // Engine fluid check
            }
        }

        private static string GetSectionCExplanation(int index)
        {
            return "Official JPJ KEJARA Demerit System & Safety Checks (Section C). Vehicle maintenance, RPK/RSM inspection, and demerit point thresholds.";
        }

        #endregion
    }
}
