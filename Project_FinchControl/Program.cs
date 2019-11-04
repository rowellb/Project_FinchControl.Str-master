using System;
using System.IO;
using System.Collections.Generic;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: User controls Finch Robot
    // Application Type: Console
    // Author: Rowell, Brynn
    // Dated Created: 10/1/19
    // Last Modified: 10/6/29
    //
    // **************************************************

    class Program
    {

        public enum Command
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            DONE   
        }

        static void Main(string[] args)
        {
            ReadTheme();
            UpdateTheme();
            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        static void SetTheme()
        {
            string dataPath = @"Data\Theme.txt";
            string foregroundColorString;
            ConsoleColor foregroundColor;

            //
            // read and convert to enum the foreground color
            //

            foregroundColorString = File.ReadAllText(dataPath);
            Enum.TryParse(foregroundColorString, out foregroundColor);

            Console.ForegroundColor = foregroundColor;
        }

        static void UpdateTheme(ConsoleColor background, ConsoleColor foreground)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
        }

        static void WriteTheme(ConsoleColor background, ConsoleColor foreground)
        {
            string dataPath = @"Data\Theme.txt";
            string foregroundColorString;
            ConsoleColor foregroundColor;

            foregroundColorString = File.WriteAllLines(dataPath);
            Enum.TryParse(foregroundColorString, out foregroundColor);

           
        }

        static (ConsoleColor background, ConsoleColor foreground) ReadTheme()
        {


            return 
        }

        static void DisplayChangeTheme()
        {

        }

        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display main menu screen 
        /// </summary>
        static void DisplayMenuScreen()
        {

            Finch finchRobot = new Finch();

            bool finchRobotConnected = false;
            bool quitApplication = false;
            ConsoleKeyInfo menuChoiceKey;
            char menuChoice;

            do
            {
                //
                // get user menu choice
                //

                DisplayScreenHeader("Menu");
                Console.WriteLine("a) Connect Finch Robot");
                Console.WriteLine("b) Talent Show");
                Console.WriteLine("c) Data Recorder");
                Console.WriteLine("d) Alarm System");
                Console.WriteLine("e) User Programming");
                Console.WriteLine("f) Disconnect Finch Bot");
                Console.WriteLine("g) Change Theme");
                Console.WriteLine("q) Quit");
                Console.WriteLine("Enter choice");
                menuChoiceKey = Console.ReadKey();
                menuChoice = menuChoiceKey.KeyChar;


                //
                // process user menu choice
                //

                switch (menuChoice)
                {
                    case 'a':
                       finchRobotConnected = DisplayConnectFinchRobot(finchRobot);
                        break;

                    case 'b':
                        if (finchRobotConnected)
                        {
                        DisplayTalentShow(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine("Finch robot is not connected. Please return to Main Menu and connect.");
                            DisplayContinuePrompt();
                        }

                        break;

                    case 'c':
                        DisplayDataRecord(finchRobot);
                        break;

                    case 'd':
                        DisplayAlarmSystem(finchRobot);
                        break;

                    case 'e':
                        DisplayUserProgramming(finchRobot);
                        break;

                    case 'f':

                        break;

                    case 'g':
                        DisplayChangeTheme();
                        break;

                    case 'q':
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;


                    default:
                        Console.WriteLine();
                        Console.WriteLine("please enter a letter for the menu choice");
                        DisplayContinuePrompt();
                        break;
                }


            } while (!quitApplication);
            finchRobot.disConnect();
        }
        #region USER PROGRAMMING

        static void DisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Commands");

            Console.WriteLine("The Chosen Finch Commands Are: ");
            Console.WriteLine();

            foreach (Command command in commands)
            {               
                Console.WriteLine(command);
            }

            DisplayContinuePrompt();
        }

        static void DisplayGetFinchCommands(List<Command> commands)
        {
            string userResponse;
            Command command = Command.NONE;           

            DisplayScreenHeader("Finch Robot Commands");

            while (command != Command.DONE)
            {
                // todo- validate
              
                   
                    Console.Write("Enter Command: ");
                    userResponse = Console.ReadLine().ToUpper();
                    Enum.TryParse(userResponse, out command);
                    commands.Add(command);
            }

            // todo - echo user response


            DisplayContinuePrompt();
        }

        static (int motorSpeed, int ledBrightness, int waitSeconds) DisplayGetCommandParameters()
        {
            string userResponse;


            // todo - maybe add temperature into tuple
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters;

            DisplayScreenHeader("Command Parameters");

            Console.Write("Enter LED Brightness [0 - 255]: ");
            userResponse = Console.ReadLine();
            commandParameters.motorSpeed = int.Parse(userResponse);

            Console.Write("Enter Motor Speed [0 - 255]: ");
            commandParameters.ledBrightness = int.Parse(Console.ReadLine());

            Console.Write("Enter Wait Command in Seconds: ");
            commandParameters.waitSeconds = int.Parse(Console.ReadLine());

            Console.WriteLine(commandParameters);

            DisplayContinuePrompt();

            return commandParameters;
        }

        static void DisplayUserProgramming(Finch finchRobot)
        {
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameter;
            commandParameter.motorSpeed = 0;
            commandParameter.ledBrightness = 0;
            commandParameter.waitSeconds = 0;

            List<Command> commands = new List<Command>();
            string menuChoice;
            bool quitApplication = false;

            do
            {

                DisplayScreenHeader("User Programming");

                Console.WriteLine("a) Set Command Parameters");
                Console.WriteLine("b) Add Commands");
                Console.WriteLine("c) View Commands");
                Console.WriteLine("d) Execute Commands");
                Console.WriteLine("e) Write Commands to Data File");
                Console.WriteLine("F) Read Commands from Data File");
                Console.WriteLine("q) Return to Main Menu");
                Console.WriteLine("Enter choice");
                menuChoice = Console.ReadLine().ToLower();

                switch (menuChoice)
                {
                    case "a":
                        commandParameter = DisplayGetCommandParameters();
                        break;

                    case "b":
                        DisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        DisplayFinchCommands(commands);
                        break;

                    case "d":
                        DisplayExecuteFinchCommands(finchRobot, commands, commandParameter);
                        break;

                    case "e":
                        DisplayWriteUserProgrammingData(commands);
                        break;

                    case "f":
                        commands = DisplayReadUserProgrammingData();
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;


                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter a menu choice");
                        DisplayContinuePrompt();
                        break;
                }


            } while (!quitApplication);
        }

        static List<Command> DisplayReadUserProgrammingData()
        {
            string dataPath = @"Data\Data.txt";
            List<Command> commands = new List<Command>();
            string[] commandsString;

            DisplayScreenHeader("Read Data From File");

            Console.WriteLine("Ready to read from the data file.");
            DisplayContinuePrompt();

            commandsString = File.ReadAllLines(dataPath);

            //
            // create list of command enum
            //
            Command command;
            foreach  (string commandString in commandsString)
            {
                Enum.TryParse(commandString, out command);
                commands.Add(command);

            }

            DisplayContinuePrompt();

            return commands;
        }

        static void DisplayWriteUserProgrammingData(List<Command> commands)
        {
            string dataPath = @"Data\Data.txt";

            DisplayScreenHeader("Write Data to File");

            List<string> commandsString = new List<string>();

            // 
            // create a list of command strings
            //

            foreach (Command command in commands)
            {
                commandsString.Add(command.ToString());
            }

            Console.WriteLine("Ready to write to the data file.");
            DisplayContinuePrompt();

            File.WriteAllLines(dataPath, commandsString.ToArray());

            DisplayContinuePrompt();
        }

        static void DisplayExecuteFinchCommands(
            Finch finchRobot,
            List<Command> commands,
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameter)
        {
            int motorSpeed = commandParameter.motorSpeed;
            int ledBrightness = commandParameter.ledBrightness;
            int waitSeconds = commandParameter.waitSeconds;
            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("The Finch Robot will now execute commands");

            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:

                        break;
                    case Command.MOVEFORWARD:
                        DisplayScreenHeader("Move Forward");
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        break;
                    case Command.MOVEBACKWARD:
                        DisplayScreenHeader("Move Backward");
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        break;
                    case Command.STOPMOTORS:
                        DisplayScreenHeader("Stop");
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.WAIT:
                        DisplayScreenHeader("Wait");
                        finchRobot.wait(waitSeconds * 1000);
                        break;
                    case Command.TURNRIGHT:
                        DisplayScreenHeader("Turn Right");
                        break;
                    case Command.TURNLEFT:
                        DisplayScreenHeader("Turn Left");
                        break;
                    case Command.LEDON:
                        DisplayScreenHeader("LED On");
                        break;
                    case Command.LEDOFF:
                        DisplayScreenHeader("LED Off");
                        break;
                    case Command.DONE:
                        DisplayScreenHeader("Done");
                        break;
                    default:
                        break;
                }

            }


            DisplayContinuePrompt();
        }

        #endregion
        private static void DisplayAlarmSystem(Finch finchRobot)
        {
            string alarmType;
            int maxSeconds;
            double threshold;
            bool thresholdExceeded;

            DisplayScreenHeader("Alarm System");

            alarmType = DisplayGetAlarmType();
            maxSeconds = DisplayGetMaxSeconds();
            threshold = DisplayGetThreshold(finchRobot, alarmType);

            Console.WriteLine("The Finch Robot Will demonstrate Upper Threshold Capabilities");
            DisplayContinuePrompt();


            if (alarmType == "light")
            {
                thresholdExceeded = MonitorLightLevels(finchRobot, threshold, maxSeconds);
            }
            else
            {
                thresholdExceeded = MonitorTemperatureLevels(finchRobot, threshold, maxSeconds);
            }


            if (thresholdExceeded)
            {
                if (alarmType == "light")
                {
                    Console.WriteLine("Maximum Light Level Exceeded");
                }
                else
                {
                    Console.WriteLine("Maximum Temperature Exceeded");
                }
            }
            else
            {
                Console.WriteLine("Maximum Monitoring Time Exceeded");
            }

            DisplayContinuePrompt();
        }

        static bool MonitorTemperatureLevels(Finch finchRobot, double threshold, int maxSeconds)
        {
            bool thresholdExceeded = false;
            double seconds = 0;
            double currentTemperature;

            while (!thresholdExceeded && seconds <= maxSeconds)
            {
                finchRobot.setLED(0, 255, 0);
                currentTemperature = finchRobot.getTemperature();
                DisplayScreenHeader("Moniter Temperature");
                Console.WriteLine($"Maximum Temperature {threshold}");
                Console.WriteLine($"Current Temperature {currentTemperature}°C");

                if (currentTemperature >= threshold)
                {
                    finchRobot.setLED(255, 0, 0);
                    thresholdExceeded = true;
                }

                finchRobot.wait(500);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }

        static bool MonitorLightLevels(Finch finchRobot, double threshold, int maxSeconds)
        {

            bool thresholdExceeded = false;
            double seconds = 0;
            int currentLightLevel;

            while (!thresholdExceeded && seconds <= maxSeconds)
            {
                finchRobot.setLED(0, 255, 0);
                currentLightLevel = finchRobot.getRightLightSensor();
                DisplayScreenHeader("Moniter Light Levels");
                Console.WriteLine($"Maximum Light Level: {(int)threshold}");
                Console.WriteLine($"Current Light Level: {currentLightLevel}");

                if (currentLightLevel >= threshold)
                {
                    finchRobot.setLED(255, 0, 0);
                    thresholdExceeded = true;
                }

                finchRobot.wait(500);
                seconds += 0.5;
            }



            return thresholdExceeded;
        }

        static double DisplayGetThreshold(Finch finchRobot, string alarmType)
        {
            double threshold = 0;

            DisplayScreenHeader("Threshold Value");

            switch (alarmType)
            {
                case "light":
                    Console.WriteLine($"Current Light Level: {finchRobot.getLeftLightSensor()}");
                    threshold = GetMaximumLightLevel();

                    break;

                case "temperature":
                    Console.WriteLine($"Current Temperature: {finchRobot.getTemperature()}°C");
                    threshold = GetMaximumTemperature();

                    break;

                default:
                    throw new FormatException();

                    break;
            }


            DisplayContinuePrompt();

            return threshold;
        }
        
        static int GetMaximumLightLevel()
        {
            bool validResponse;
            int light;


            do
            {
                validResponse = true;
                Console.Write("Enter Maximum Light Level [above 0]: ");
                light = int.Parse(Console.ReadLine());
               


                if (light <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please Try Again");
                    validResponse = false;
                }

            } while (!validResponse);

            return light;
        }

        static int GetMaximumTemperature()
        {
            bool validResponse;
            int temperature;


            do
            {
                validResponse = true;
                Console.Write("Enter Maximum Temperature [above 0]:");
                temperature = int.Parse(Console.ReadLine());


                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please Try Again");
                    validResponse = false;
                }

            } while (!validResponse);

            return temperature;
        }



        static int DisplayGetMaxSeconds()
        {
            bool validResponse;
            string userResponse;
            int seconds;

            do
            {
                validResponse = true;
                Console.Write("Enter Max seconds [above 0]: ");
                userResponse = Console.ReadLine();
                seconds = int.Parse(userResponse);

                if (seconds <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please Try Again");
                    validResponse = false;
                }

            } while (!validResponse);
            
            return seconds;
        }

        static string DisplayGetAlarmType()
        {
            bool validResponse;
            string userResponse;


            do
            {
                validResponse = true;
                Console.Write("Alarm Type [light or temperature]:");
                userResponse = Console.ReadLine();


                if (userResponse == "light")
                {
                    validResponse = true;
                }
                else if (userResponse == "temperature")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("please try again");
                    validResponse = false;
                }


            } while (!validResponse);

            return userResponse;
        }

        /// <summary>
        /// Displays tmperature data
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void DisplayDataRecord(Finch finchRobot)
        {
            double frequency;
            int numberOfDataPoints; 

            DisplayScreenHeader("Data Recorder");
            Console.WriteLine("The Finch Robot Will Record the Tmperature of its Surroundings");

            frequency = DisplayGetDataRecorderFrequency(finchRobot);
            numberOfDataPoints = DisplayGetNumberOfDataPoints(finchRobot);


            //
            // instanciate (create) array
            //
            double[] temperatures = new double[numberOfDataPoints];


            // warn user before recording
            DisplayGetDataReadings(numberOfDataPoints, frequency, temperatures, finchRobot);

            DisplayDataRecorderData(temperatures);

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Get the frequency of recording
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        static double DisplayGetDataRecorderFrequency(Finch finchRobot)
        {
            string userResponse;
            double frequency;
            bool validResponse;

            DisplayScreenHeader("Get Frequency of Recording");


            double.TryParse(Console.ReadLine(), out frequency);

            do
            {
                validResponse = true;
                Console.Write("Enter Frequency (seconds greater than or equal to 1)");
                userResponse = Console.ReadLine();
                double.TryParse(userResponse, out frequency);


                if (frequency <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please provide a number");
                    validResponse = false;
                }

            } while (!validResponse);

            DisplayContinuePrompt();

            return frequency;

        }

        /// <summary>
        /// Ge tthe number of recordings
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        static int DisplayGetNumberOfDataPoints(Finch finchRobot)
        {
            string userResponse;
            int numberOfDataPoints;
            bool validResponse; 

            DisplayScreenHeader("Get Number of Recordings");


            do
            {
                validResponse = true;
                Console.Write("Enter the number of data points (greater than 0). ");
                userResponse = Console.ReadLine();
                int.TryParse(userResponse, out numberOfDataPoints);


                if (numberOfDataPoints <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please provide a number");
                    validResponse = false;
                }

            } while (!validResponse);


            DisplayContinuePrompt();
            return numberOfDataPoints;

            


        }

        /// <summary>
        /// Display the data readings 
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="frequencyOfDataPoints"></param>
        /// <param name="temperatures"></param>
        /// <param name="finchRobot"></param>
        static void DisplayGetDataReadings(int numberOfDataPoints, double frequencyOfDataPoints, double[] temperatures, Finch finchRobot)
        {
            DisplayScreenHeader("Get Temperature Recorderdings");

            // prompt the user
            
            //
            // get temperatures
            //

            for (int index = 0; index < numberOfDataPoints; index++)
            {

                temperatures[index] = finchRobot.getTemperature();
                int milleseconds = (int)(frequencyOfDataPoints * 1000);
                finchRobot.wait(milleseconds);
                Console.WriteLine($"temperature {index +1}: {temperatures[index]}°C");

            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Display the temperature array
        /// </summary>
        /// <param name="temperatures"></param>
        static void DisplayDataRecorderData(double[] temperatures)
        {
            DisplayScreenHeader("Temperatures");

            // provide information to the user
            Console.WriteLine("Data set");
            Console.WriteLine();

            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine($"Temperature {index + 1}: {(temperatures[index] * 1.8) + 32}°F");
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// finch Robot will perform a talent show 
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplayTalentShow( Finch finchRobot)
        {
            string noseColor;
            bool validColor;

            DisplayScreenHeader("Talent Show");

            Console.WriteLine("The Finch robot will now show off its talent");
            DisplayContinuePrompt();

            for (int lightLevel = 0; lightLevel < 255; lightLevel++)
            {
                finchRobot.setLED(lightLevel, lightLevel, lightLevel);
            }

            validColor = false;

            do
            {
                Console.WriteLine("What color would you like the finch nose to turn first? (red , green, blue) ");
                noseColor = Console.ReadLine().ToLower();
                if (noseColor == "red")
                {
                    validColor = true;
                    finchRobot.setLED(255, 0, 0);
                }
                else if (noseColor == "green")
                {
                    validColor = true;
                    finchRobot.setLED(0, 255, 0);
                }
                else if (noseColor == "blue")
                {
                    validColor = true;
                }
                else
                {
                    Console.WriteLine("answer is invalid, please try again and type red, green, or blue. ");
                }
            } while (!validColor);

            Console.WriteLine("The Finch robot will now move forward at full speed for one second");
            DisplayContinuePrompt();

            finchRobot.setMotors(255, 255);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);

            Console.WriteLine("The Finch robot will now move backward at full speed for one second");
            DisplayContinuePrompt();

            finchRobot.setMotors(-255, -255);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);

            Console.WriteLine("The Finch robot will now move in a circle at full speed for two seconds and play two different tones for 2 seconds");
            DisplayContinuePrompt();

            finchRobot.noteOn(995);
            finchRobot.wait(1000);
            finchRobot.noteOn(600);
            finchRobot.wait(1000);
            finchRobot.setMotors(-255, 255);
            finchRobot.wait(3000);
            finchRobot.setMotors(0, 0);
            finchRobot.noteOff();
            finchRobot.noteOff();

            Console.WriteLine("The Finch robot will now perform a dance");
            DisplayContinuePrompt();

            finchRobot.setLED(0, 0, 255);
            finchRobot.setMotors(255, -255);
            finchRobot.wait(3000);
            finchRobot.setMotors(0, 0);
            finchRobot.setMotors(-255, 255);
            finchRobot.wait(3000);
            finchRobot.setMotors(0, 0);
            finchRobot.setMotors(155, 155);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 0);
            finchRobot.setMotors(-155, -155);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 0);

            Console.WriteLine("The Finch robot will play a series of tones");
            DisplayContinuePrompt();

            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(361);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.setLED(0, 255, 0);
            finchRobot.noteOn(461);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 255);
            finchRobot.noteOn(400);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(600);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.setLED(0, 255, 0);
            finchRobot.noteOn(361);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 255);
            finchRobot.noteOn(461);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(400);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.setLED(0, 255, 0);
            finchRobot.noteOn(600);
            finchRobot.wait(500);
            finchRobot.noteOff();

            Console.WriteLine("Talent Show completed");
            DisplayContinuePrompt();
        }

        /// <summary>
        /// connect to finch robot
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            bool robotConnected;

            DisplayScreenHeader("Connect to Finch Robot");

            Console.WriteLine("About to connect to finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            if (robotConnected)
            {
                Console.WriteLine("The Finch robot is now connected,");
                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(15000);
                finchRobot.wait(1000);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine("unable to connect to the Finch robot.");
            }

            DisplayContinuePrompt();

            return robotConnected;
        }

        /// <summary>
        /// display disconnect finch robot
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            DisplayScreenHeader("Disconnect FInch Robot");

            Console.WriteLine("About to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("The Finch robot is now disconnected.");

            DisplayContinuePrompt();
        }
        #region HELPER METHODS

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }


        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
