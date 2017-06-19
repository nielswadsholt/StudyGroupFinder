using Graph;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using StudyGroupFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Util;

namespace StudyGroupFinderBot.Dialogs
{
    [LuisModel("", "")]
    [Serializable]
    public class SGFLuisDialog : LuisDialog<object>
    {
        private static Digraph<Student> digraph = StudentGraphGenerator.Sample();
        private static Student currentStudent;

        [LuisIntent("Restart")]
        public async Task Restart(IDialogContext context, LuisResult result)
        {
            digraph = StudentGraphGenerator.Sample();
            await context.PostAsync($"The system has been restarted.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("StudentCount")]
        public async Task StudentCount(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"There are currently {digraph.Nodes.Count} students registered in the system.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("StudentList")]
        public async Task StudentList(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Registered students: { digraph.Nodes.Keys.ToSeparatedString(", ") }.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("StudentDetails")]
        public async Task StudentDetails(IDialogContext context, LuisResult result)
        {
            string student = "";
            EntityRecommendation rec;

            if (result.TryFindEntity("Student", out rec))
            {
                try
                {
                    student = rec.Entity.Substring(0, 1).ToUpper() + rec.Entity.Substring(1).ToLower();

                    if (digraph.Contains(student))
                    {
                        await context.PostAsync($"{ digraph[student].Data.GetDetails() }, Neighbors: [{ digraph[student].Neighbors.ToSeparatedString() }].");
                    }
                    else
                    {
                        await context.PostAsync("The system does not contain a student with the given name.");
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync($"Whoops, something went wrong. Please try again. Details: { ex.Message }");
                }
            }
            else
            {
                await context.PostAsync("The student name was not recognized. Please try again.");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("RemoveStudent")]
        public async Task RemoveStudent(IDialogContext context, LuisResult result)
        {
            string student = "";
            EntityRecommendation rec;

            if (result.TryFindEntity("Student", out rec))
            {
                try
                {
                    student = rec.Entity.Substring(0, 1).ToUpper() + rec.Entity.Substring(1).ToLower();

                    if (digraph.Contains(student))
                    {
                        digraph.RemoveNode(student);
                        await context.PostAsync($" { student } has been removed from the system.");
                    }
                    else
                    {
                        await context.PostAsync("The system does not contain a student with the given name.");
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync($"Whoops, something went wrong. Please try again. Details: { ex.Message }");
                }
            }
            else
            {
                await context.PostAsync("The student name was not recognized. Please try again.");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("Find")]
        public async Task Find(IDialogContext context, LuisResult result)
        {
            string student = "";
            EntityRecommendation rec;            

            try
            {
                if (result.TryFindEntity("Student", out rec))
                {
                    student = rec.Entity.Substring(0, 1).ToUpper() + rec.Entity.Substring(1).ToLower();

                    if (digraph.Contains(student))
                    {
                        Path path = digraph.FindPath(currentStudent.Name, student);

                        if (path.Count > 0)
                        {
                            await context.PostAsync($"Path from { currentStudent } to { student }: { path }.");
                        }
                        else
                        {
                            await context.PostAsync($"There is no path between { currentStudent } and { student }.");
                        }
                    }
                    else
                    {
                        await context.PostAsync("The system does not contain a student with the given name.");
                    }
                }
                else if (result.Query.Contains("group"))
                {
                    string study = currentStudent.Study;

                    Path path = digraph.FindShortestPath(currentStudent.Name, s => s.SeeksGroup && s.Study == study);

                    if (path.Count > 0)
                    {
                        await context.PostAsync($"Path from { currentStudent } to closest study group prospect: { path }.");
                    }
                    else
                    {
                        await context.PostAsync($"Sorry, there is no path from { currentStudent } to a student who also studies { study.ToLower() } and seeks a study group.");
                    }
                }
                else if (result.Query.Contains("same study"))
                {
                    string study = currentStudent.Study;

                    Path path = digraph.FindShortestPath(currentStudent.Name, s => s.Study == study);

                    if (path.Count > 0)
                    {
                        await context.PostAsync($"Path from { currentStudent } to closest student who also studies { study.ToLower() }: { path }.");
                    }
                    else
                    {
                        await context.PostAsync($"There is no path from { currentStudent } to a student who also studies { study.ToLower() }.");
                    }
                }
                else
                {
                    await context.PostAsync("Sorry, I didn't understand. Please try again.");
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Whoops, something went wrong. Please try again. Details: { ex.Message }");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("GetCurrent")]
        public async Task GetCurrent(IDialogContext context, LuisResult result)
        {
            if (currentStudent is null)
            {
                await context.PostAsync("No student is currently selected.");
            }
            else
            {
                await context.PostAsync($"The currently selected student is { currentStudent.Name }.");
            }

            
            context.Wait(MessageReceived);
        }

        [LuisIntent("SetCurrent")]
        public async Task SetCurrent(IDialogContext context, LuisResult result)
        {
            string student = "";
            EntityRecommendation rec;

            if (result.TryFindEntity("Student", out rec))
            {
                try
                {
                    student = rec.Entity.Substring(0, 1).ToUpper() + rec.Entity.Substring(1).ToLower();

                    if (digraph.Contains(student))
                    {
                        currentStudent = digraph[student].Data;
                        await context.PostAsync($"The current student has been set to { student }");
                    }
                    else
                    {
                        await context.PostAsync("The system does not contain a student with the given name.");
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync($"Whoops, something went wrong. Please try again. Details: { ex.Message }");
                }
            }
            else
            {
                await context.PostAsync("The student name was not recognized. Please try again.");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("ChangePathFinder")]
        public async Task ChangePathFinder(IDialogContext context, LuisResult result)
        {
            string pathFinder = "";
            EntityRecommendation rec;

            if (result.TryFindEntity("PathFinder", out rec))
            {
                try
                {
                    pathFinder = rec.Entity.ToLower();

                    if (pathFinder == "bfs")
                    {
                        digraph.PathFinder = new BFS<Student>();
                    }
                    else if (pathFinder == "dfs")
                    {
                        digraph.PathFinder = new DFS<Student>();
                    }

                    await context.PostAsync($"The pathfinder algorithm has been set to { digraph.PathFinder }");
                }
                catch (Exception)
                {
                    await context.PostAsync("Sorry, there is no pathfinder algorithm with the given name. Try BFS or DFS.");
                }
            }
            else
            {
                await context.PostAsync("Sorry, there is no pathfinder algorithm with the given name. Try BFS or DFS.");
            }
            
            context.Wait(MessageReceived);
        }

        [LuisIntent("DeepThought")]
        public async Task DeepThought(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("42");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Help is under construction.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I did not understand. Please try again.");
            context.Wait(MessageReceived);
        }
    }
}