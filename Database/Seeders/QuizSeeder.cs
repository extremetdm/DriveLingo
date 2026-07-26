using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DriveLingo.Database.Seeders
{
    public static class QuizSeeder
    {
        public static void Run(AppDbContext db)
        {
            var quiz1 = new Quiz
            {
                Id = 1,
                ModuleId = 1,
                Title = "JPJ Road Signs Quiz",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "What does the road sign shown below indicate?",
                        Image = "/uploads/no_entry.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "No Parking Zone", IsCorrect = false },
                            new QuestionChoice { Text = "No Entry (Dilarang Masuk)", IsCorrect = true },
                            new QuestionChoice { Text = "Stop Command", IsCorrect = false },
                            new QuestionChoice { Text = "Speed Limit Ahead", IsCorrect = false }
                        }
                    },
                    new Question
                    {
                        Text = "What does this yellow diamond road sign warn drivers about?",
                        Image = "/uploads/warning_curve.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Slippery Road", IsCorrect = false },
                            new QuestionChoice { Text = "Sharp Right Curve Ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Narrow Bridge", IsCorrect = false },
                            new QuestionChoice { Text = "Roundabout Ahead", IsCorrect = false }
                        }
                    },
                    new Question
                    {
                        Text = "What type of highway route is indicated by the green background sign below?",
                        Image = "/uploads/expressway_sign.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "State Highway", IsCorrect = false },
                            new QuestionChoice { Text = "Malaysian Expressway (Lebuhraya)", IsCorrect = true },
                            new QuestionChoice { Text = "Federal Route", IsCorrect = false },
                            new QuestionChoice { Text = "Town Municipal Road", IsCorrect = false }
                        }
                    },
                    new Question
                    {
                        Text = "What does this NO PARKING sign indicate?",
                        Image = "uploads/no_parking.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Parking allowed for 30 minutes", IsCorrect = false },
                            new QuestionChoice { Text = "No parking at any time", IsCorrect = true },
                            new QuestionChoice { Text = "Loading zone only", IsCorrect = false },
                            new QuestionChoice { Text = "No stopping", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this NO STOPPING sign indicate?",
                        Image = "uploads/no_stopping.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Parking is prohibited", IsCorrect = false },
                            new QuestionChoice { Text = "Stopping is prohibited", IsCorrect = true },
                            new QuestionChoice { Text = "No U-turn", IsCorrect = false },
                            new QuestionChoice { Text = "Keep moving at all times", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this TWO-WAY TRAFFIC sign warn drivers about?",
                        Image = "uploads/two_way_traffic.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "One-way road ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Two-way traffic ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Road closed", IsCorrect = false },
                            new QuestionChoice { Text = "Divided highway", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this DIVIDED HIGHWAY sign indicate?",
                        Image = "uploads/divided_highway.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Road narrows ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Divided highway begins", IsCorrect = true },
                            new QuestionChoice { Text = "Bridge ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Tunnel ahead", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this CROSSROAD warning sign indicate?",
                        Image = "uploads/crossroad.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Roundabout ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Crossroad ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Dead end", IsCorrect = false },
                            new QuestionChoice { Text = "Railway crossing", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this T-JUNCTION sign warn drivers about?",
                        Image = "uploads/t_junction.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Road ends at a T-junction", IsCorrect = true },
                            new QuestionChoice { Text = "Crossroad ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Y-junction ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Roundabout ahead", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this Y-JUNCTION sign warn drivers about?",
                        Image = "uploads/y_junction.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "T-junction ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Road splits into two directions", IsCorrect = true },
                            new QuestionChoice { Text = "Dead end", IsCorrect = false },
                            new QuestionChoice { Text = "Road narrows", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this LEFT CURVE warning sign indicate?",
                        Image = "uploads/left_curve.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Sharp left curve ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Right curve ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Road ends", IsCorrect = false },
                            new QuestionChoice { Text = "U-turn permitted", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this DOUBLE BEND sign warn drivers about?",
                        Image = "uploads/double_bend.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Two consecutive bends ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Two-way traffic", IsCorrect = false },
                            new QuestionChoice { Text = "Roundabout ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Bridge ahead", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this TRAFFIC SIGNAL AHEAD sign warn drivers about?",
                        Image = "uploads/traffic_signal_ahead.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Railway crossing ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Traffic lights ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Pedestrian crossing", IsCorrect = false },
                            new QuestionChoice { Text = "Police checkpoint", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this ANIMAL CROSSING sign warn drivers about?",
                        Image = "uploads/animal_crossing.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Zoo entrance", IsCorrect = false },
                            new QuestionChoice { Text = "Animals may cross the road", IsCorrect = true },
                            new QuestionChoice { Text = "Farm nearby", IsCorrect = false },
                            new QuestionChoice { Text = "No livestock allowed", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this BICYCLE CROSSING sign indicate?",
                        Image = "uploads/bicycle_crossing.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "No bicycles allowed", IsCorrect = false },
                            new QuestionChoice { Text = "Cyclists may cross ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Bicycle parking", IsCorrect = false },
                            new QuestionChoice { Text = "Motorcycle lane", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this BUS STOP sign indicate?",
                        Image = "uploads/bus_stop.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Taxi stand", IsCorrect = false },
                            new QuestionChoice { Text = "Bus stop ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Parking area", IsCorrect = false },
                            new QuestionChoice { Text = "Bus lane ends", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this TUNNEL sign indicate?",
                        Image = "uploads/tunnel.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Bridge ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Tunnel ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Road narrows", IsCorrect = false },
                            new QuestionChoice { Text = "Expressway exit", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this LOW CLEARANCE sign warn drivers about?",
                        Image = "uploads/low_clearance.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Steep hill ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Low height restriction ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Bridge closed", IsCorrect = false },
                            new QuestionChoice { Text = "Road narrows", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this LANE ENDS sign warn drivers about?",
                        Image = "uploads/lane_ends.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Road closed", IsCorrect = false },
                            new QuestionChoice { Text = "A traffic lane ends ahead", IsCorrect = true },
                            new QuestionChoice { Text = "One-way traffic", IsCorrect = false },
                            new QuestionChoice { Text = "Parking lane", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does this CHEVRON sign help drivers identify?",
                        Image = "uploads/chevron.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Location of a fuel station", IsCorrect = false },
                            new QuestionChoice { Text = "Direction of a sharp bend", IsCorrect = true },
                            new QuestionChoice { Text = "Speed limit", IsCorrect = false },
                            new QuestionChoice { Text = "Parking area", IsCorrect = false }
                        }
                    },

                }
            };

            var quiz2 = new Quiz
            {
                Id = 2,
                ModuleId = 2,
                Title = "JPJ Road Rules Quiz",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "What should a driver do before changing lanes on a highway?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Immediately move into another lane", IsCorrect = false },
                            new QuestionChoice { Text = "Check mirrors, signal and ensure it is safe", IsCorrect = true },
                            new QuestionChoice { Text = "Increase speed without checking", IsCorrect = false },
                            new QuestionChoice { Text = "Sound the horn continuously", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "In Malaysia, drivers should normally drive on which side of the road?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Right side", IsCorrect = false },
                            new QuestionChoice { Text = "Left side", IsCorrect = true },
                            new QuestionChoice { Text = "Middle of the road", IsCorrect = false },
                            new QuestionChoice { Text = "Any side available", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "When approaching a zebra crossing, what should a driver do?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Speed up before pedestrians cross", IsCorrect = false },
                            new QuestionChoice { Text = "Slow down and give way to pedestrians", IsCorrect = true },
                            new QuestionChoice { Text = "Overtake other vehicles", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore pedestrians", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is the purpose of using a vehicle signal indicator?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To warn other road users about your intention", IsCorrect = true },
                            new QuestionChoice { Text = "To increase vehicle speed", IsCorrect = false },
                            new QuestionChoice { Text = "To replace the horn", IsCorrect = false },
                            new QuestionChoice { Text = "To stop other vehicles", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "When driving during heavy rain, what should a driver do?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Increase speed to leave the rain area", IsCorrect = false },
                            new QuestionChoice { Text = "Reduce speed and increase following distance", IsCorrect = true },
                            new QuestionChoice { Text = "Switch off headlights", IsCorrect = false },
                            new QuestionChoice { Text = "Drive close behind another vehicle", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is the correct action when an emergency vehicle approaches with siren and flashing lights?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Block the road to stop it", IsCorrect = false },
                            new QuestionChoice { Text = "Give way and allow it to pass", IsCorrect = true },
                            new QuestionChoice { Text = "Follow closely behind it", IsCorrect = false },
                            new QuestionChoice { Text = "Race with the emergency vehicle", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver check before starting a journey?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Only the fuel level", IsCorrect = false },
                            new QuestionChoice { Text = "Vehicle condition including tyres, lights and brakes", IsCorrect = true },
                            new QuestionChoice { Text = "Only the radio", IsCorrect = false },
                            new QuestionChoice { Text = "Only the air conditioner", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What does a solid double line on the road generally indicate?",
                        Image = "uploads/double_line.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Overtaking is allowed anytime", IsCorrect = false },
                            new QuestionChoice { Text = "Do not cross the line unless permitted", IsCorrect = true },
                            new QuestionChoice { Text = "Parking area", IsCorrect = false },
                            new QuestionChoice { Text = "Bus lane only", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "When approaching a junction without traffic lights, what should drivers do?",
                        Image = "uploads/junction.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Maintain high speed", IsCorrect = false },
                            new QuestionChoice { Text = "Slow down and observe traffic conditions", IsCorrect = true },
                            new QuestionChoice { Text = "Ignore other vehicles", IsCorrect = false },
                            new QuestionChoice { Text = "Overtake immediately", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is the safest following distance between vehicles?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Very close distance", IsCorrect = false },
                            new QuestionChoice { Text = "Enough distance to stop safely", IsCorrect = true },
                            new QuestionChoice { Text = "One metre only", IsCorrect = false },
                            new QuestionChoice { Text = "No distance needed", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver do when feeling sleepy while driving?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Continue driving faster", IsCorrect = false },
                            new QuestionChoice { Text = "Stop at a safe place and rest", IsCorrect = true },
                            new QuestionChoice { Text = "Drink water while driving", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore tiredness", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "When overtaking another vehicle, the driver should ensure that:",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "The road ahead is clear and safe", IsCorrect = true },
                            new QuestionChoice { Text = "The vehicle ahead is slow only", IsCorrect = false },
                            new QuestionChoice { Text = "There are no traffic rules", IsCorrect = false },
                            new QuestionChoice { Text = "The horn is used continuously", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is the purpose of a vehicle rear-view mirror?",
                        Image = "uploads/rear_view_mirror.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To check traffic behind the vehicle", IsCorrect = true },
                            new QuestionChoice { Text = "To increase speed", IsCorrect = false },
                            new QuestionChoice { Text = "To replace indicators", IsCorrect = false },
                            new QuestionChoice { Text = "To control brakes", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "When parking on a slope, a driver should:",
                        Image = "uploads/slope_parking.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Ensure the vehicle cannot roll away", IsCorrect = true },
                            new QuestionChoice { Text = "Leave the engine running", IsCorrect = false },
                            new QuestionChoice { Text = "Remove the steering wheel", IsCorrect = false },
                            new QuestionChoice { Text = "Park without applying brakes", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver do before making a U-turn?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Check whether it is allowed and safe", IsCorrect = true },
                            new QuestionChoice { Text = "Turn immediately", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore road signs", IsCorrect = false },
                            new QuestionChoice { Text = "Stop in the middle of the road", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why should drivers avoid using mobile phones while driving?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "It reduces attention and increases accident risk", IsCorrect = true },
                            new QuestionChoice { Text = "It improves driving skills", IsCorrect = false },
                            new QuestionChoice { Text = "It saves fuel", IsCorrect = false },
                            new QuestionChoice { Text = "It improves visibility", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver do when approaching a school area?",
                        Image = "uploads/school_area.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Reduce speed and watch for children", IsCorrect = true },
                            new QuestionChoice { Text = "Overtake quickly", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore pedestrians", IsCorrect = false },
                            new QuestionChoice { Text = "Drive on the road shoulder", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is the purpose of a vehicle brake system?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To slow down or stop the vehicle safely", IsCorrect = true },
                            new QuestionChoice { Text = "To increase engine power", IsCorrect = false },
                            new QuestionChoice { Text = "To control headlights", IsCorrect = false },
                            new QuestionChoice { Text = "To change vehicle colour", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should drivers do when entering a roundabout?",
                        Image = "uploads/roundabout_entry.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Give priority to vehicles already in the roundabout", IsCorrect = true },
                            new QuestionChoice { Text = "Enter without checking", IsCorrect = false },
                            new QuestionChoice { Text = "Always stop inside the roundabout", IsCorrect = false },
                            new QuestionChoice { Text = "Drive against traffic direction", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why is wearing a seat belt important?",
                        Image = "uploads/seat_belt.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To reduce injuries during accidents", IsCorrect = true },
                            new QuestionChoice { Text = "To increase vehicle speed", IsCorrect = false },
                            new QuestionChoice { Text = "To save fuel", IsCorrect = false },
                            new QuestionChoice { Text = "To improve engine performance", IsCorrect = false }
                        }
                    },
                }
            };

            var quiz3 = new Quiz
            {
                Id = 3,
                ModuleId = 3,
                Title = "JPJ Kejara & Safety Quiz",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "What is the purpose of the KEJARA system in Malaysia?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To reward drivers with free vehicle services", IsCorrect = false },
                            new QuestionChoice { Text = "To record and penalise traffic offences using demerit points", IsCorrect = true },
                            new QuestionChoice { Text = "To replace driving licences", IsCorrect = false },
                            new QuestionChoice { Text = "To reduce vehicle prices", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What happens when a driver accumulates too many KEJARA demerit points?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "The driver receives a new vehicle", IsCorrect = false },
                            new QuestionChoice { Text = "The driving licence may be suspended or cancelled", IsCorrect = true },
                            new QuestionChoice { Text = "The driver can ignore future offences", IsCorrect = false },
                            new QuestionChoice { Text = "The driver gets higher speed limits", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why are traffic offences recorded under the KEJARA system?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To improve road safety and encourage responsible driving", IsCorrect = true },
                            new QuestionChoice { Text = "To increase vehicle sales", IsCorrect = false },
                            new QuestionChoice { Text = "To reduce road signs", IsCorrect = false },
                            new QuestionChoice { Text = "To remove driving tests", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver do after being involved in a road accident?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Leave immediately without reporting", IsCorrect = false },
                            new QuestionChoice { Text = "Stop safely and provide necessary assistance", IsCorrect = true },
                            new QuestionChoice { Text = "Hide the vehicle", IsCorrect = false },
                            new QuestionChoice { Text = "Continue driving normally", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is the safest action if a driver feels tired during a journey?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Open the window and continue driving fast", IsCorrect = false },
                            new QuestionChoice { Text = "Stop at a safe place and rest", IsCorrect = true },
                            new QuestionChoice { Text = "Drive without looking ahead", IsCorrect = false },
                            new QuestionChoice { Text = "Increase music volume only", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why must drivers avoid driving under the influence of alcohol?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Alcohol improves concentration", IsCorrect = false },
                            new QuestionChoice { Text = "Alcohol reduces reaction ability and judgement", IsCorrect = true },
                            new QuestionChoice { Text = "Alcohol improves vision", IsCorrect = false },
                            new QuestionChoice { Text = "Alcohol prevents accidents", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why is speeding dangerous?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "It increases reaction time", IsCorrect = false },
                            new QuestionChoice { Text = "It reduces the ability to control the vehicle safely", IsCorrect = true },
                            new QuestionChoice { Text = "It saves more fuel", IsCorrect = false },
                            new QuestionChoice { Text = "It reduces traffic accidents", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why should passengers wear seat belts?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To prevent movement inside the vehicle during a crash", IsCorrect = true },
                            new QuestionChoice { Text = "To increase vehicle speed", IsCorrect = false },
                            new QuestionChoice { Text = "To improve fuel efficiency", IsCorrect = false },
                            new QuestionChoice { Text = "To avoid traffic jams", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver do before allowing children to travel in a vehicle?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Ensure children are properly secured", IsCorrect = true },
                            new QuestionChoice { Text = "Allow children to stand freely", IsCorrect = false },
                            new QuestionChoice { Text = "Allow children to sit on the dashboard", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore passenger safety", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is defensive driving?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Driving aggressively to reach faster", IsCorrect = false },
                            new QuestionChoice { Text = "Driving carefully by anticipating possible hazards", IsCorrect = true },
                            new QuestionChoice { Text = "Ignoring other road users", IsCorrect = false },
                            new QuestionChoice { Text = "Driving without following rules", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver do when another vehicle follows too closely?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Brake suddenly", IsCorrect = false },
                            new QuestionChoice { Text = "Remain calm and allow a safe opportunity for the vehicle to pass", IsCorrect = true },
                            new QuestionChoice { Text = "Race with the vehicle", IsCorrect = false },
                            new QuestionChoice { Text = "Block the road", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why should drivers check blind spots before changing lanes?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To identify vehicles that mirrors may not show", IsCorrect = true },
                            new QuestionChoice { Text = "To increase engine power", IsCorrect = false },
                            new QuestionChoice { Text = "To reduce tyre pressure", IsCorrect = false },
                            new QuestionChoice { Text = "To avoid using indicators", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a motorcyclist wear for protection?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Approved helmet and suitable protective equipment", IsCorrect = true },
                            new QuestionChoice { Text = "Only sunglasses", IsCorrect = false },
                            new QuestionChoice { Text = "Loose clothing only", IsCorrect = false },
                            new QuestionChoice { Text = "No safety equipment", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why should drivers maintain their vehicle regularly?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To ensure the vehicle remains safe to operate", IsCorrect = true },
                            new QuestionChoice { Text = "To increase traffic offences", IsCorrect = false },
                            new QuestionChoice { Text = "To avoid using indicators", IsCorrect = false },
                            new QuestionChoice { Text = "To remove road rules", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is the correct action when approaching an emergency situation on the road?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Slow down and assess the situation safely", IsCorrect = true },
                            new QuestionChoice { Text = "Drive faster through the area", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore warning signs", IsCorrect = false },
                            new QuestionChoice { Text = "Stop in the middle of the road", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why should drivers avoid aggressive driving behaviour?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "It increases accident risk and creates danger for others", IsCorrect = true },
                            new QuestionChoice { Text = "It improves road discipline", IsCorrect = false },
                            new QuestionChoice { Text = "It reduces stress for everyone", IsCorrect = false },
                            new QuestionChoice { Text = "It improves fuel consumption", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver do when approaching a road accident scene?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Slow down and avoid causing another accident", IsCorrect = true },
                            new QuestionChoice { Text = "Stop to take unnecessary photos", IsCorrect = false },
                            new QuestionChoice { Text = "Drive against traffic flow", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore the situation completely", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why is using a mobile phone while driving unsafe?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "It distracts the driver from controlling the vehicle", IsCorrect = true },
                            new QuestionChoice { Text = "It improves reaction time", IsCorrect = false },
                            new QuestionChoice { Text = "It prevents accidents", IsCorrect = false },
                            new QuestionChoice { Text = "It improves road awareness", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should drivers do when feeling angry or stressed while driving?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Stay calm and control emotions", IsCorrect = true },
                            new QuestionChoice { Text = "Drive aggressively", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore traffic rules", IsCorrect = false },
                            new QuestionChoice { Text = "Challenge other drivers", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What is the main purpose of road safety rules?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To protect all road users and reduce accidents", IsCorrect = true },
                            new QuestionChoice { Text = "To make driving more difficult", IsCorrect = false },
                            new QuestionChoice { Text = "To increase vehicle speed", IsCorrect = false },
                            new QuestionChoice { Text = "To remove driver responsibility", IsCorrect = false }
                        }
                    },
                }
            };

            var quiz4 = new Quiz
            {
                Id = 4,
                ModuleId = 4,
                Title = "JPJ Color Blind Quiz",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "What colour is shown in this traffic light image?",
                        Image = "uploads/color_test_red_light.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Red", IsCorrect = true },
                            new QuestionChoice { Text = "Green", IsCorrect = false },
                            new QuestionChoice { Text = "Yellow", IsCorrect = false },
                            new QuestionChoice { Text = "Blue", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What action should a driver take when the traffic light shows red?",
                        Image = "uploads/color_test_red_light.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Stop before the stop line", IsCorrect = true },
                            new QuestionChoice { Text = "Proceed immediately", IsCorrect = false },
                            new QuestionChoice { Text = "Overtake other vehicles", IsCorrect = false },
                            new QuestionChoice { Text = "Increase speed", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What colour is shown in this traffic light image?",
                        Image = "uploads/color_test_green_light.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Green", IsCorrect = true },
                            new QuestionChoice { Text = "Red", IsCorrect = false },
                            new QuestionChoice { Text = "Yellow", IsCorrect = false },
                            new QuestionChoice { Text = "Orange", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What should a driver do when the traffic light changes to yellow?",
                        Image = "uploads/color_test_yellow_light.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Prepare to stop if it is safe to do so", IsCorrect = true },
                            new QuestionChoice { Text = "Always accelerate", IsCorrect = false },
                            new QuestionChoice { Text = "Reverse the vehicle", IsCorrect = false },
                            new QuestionChoice { Text = "Ignore the signal", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Which colour combination is commonly used on Malaysian road warning signs?",
                        Image = "uploads/warning_sign_colour.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Yellow and black", IsCorrect = true },
                            new QuestionChoice { Text = "Pink and white", IsCorrect = false },
                            new QuestionChoice { Text = "Purple and blue", IsCorrect = false },
                            new QuestionChoice { Text = "Brown and grey", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Which colour is normally used for prohibition road signs in Malaysia?",
                        Image = "uploads/prohibition_sign_colour.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Red", IsCorrect = true },
                            new QuestionChoice { Text = "Green", IsCorrect = false },
                            new QuestionChoice { Text = "Blue", IsCorrect = false },
                            new QuestionChoice { Text = "White only", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What number is visible in this colour vision test image?",
                        Image = "uploads/ishihara_test_01.png",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "12", IsCorrect = true },
                            new QuestionChoice { Text = "8", IsCorrect = false },
                            new QuestionChoice { Text = "6", IsCorrect = false },
                            new QuestionChoice { Text = "9", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What number is visible in this colour vision test image?",
                        Image = "uploads/ishihara_test_02.png",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "8", IsCorrect = true },
                            new QuestionChoice { Text = "3", IsCorrect = false },
                            new QuestionChoice { Text = "5", IsCorrect = false },
                            new QuestionChoice { Text = "7", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "What number is visible in this colour vision test image?",
                        Image = "uploads/ishihara_test_03.png",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "29", IsCorrect = true },
                            new QuestionChoice { Text = "70", IsCorrect = false },
                            new QuestionChoice { Text = "21", IsCorrect = false },
                            new QuestionChoice { Text = "15", IsCorrect = false }
                        }
                    },

                    new Question
                    {
                        Text = "Why is colour vision important for drivers?",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "To correctly identify traffic lights and road signals", IsCorrect = true },
                            new QuestionChoice { Text = "To increase vehicle speed", IsCorrect = false },
                            new QuestionChoice { Text = "To improve engine performance", IsCorrect = false },
                            new QuestionChoice { Text = "To reduce fuel consumption", IsCorrect = false }
                        }
                    },
                }
            };

            db.Quizzes.AddOrUpdate(quiz1);
            db.Quizzes.AddOrUpdate(quiz2);
            db.Quizzes.AddOrUpdate(quiz3);
            db.Quizzes.AddOrUpdate(quiz4);
            db.SaveChanges();
        }
    }
}