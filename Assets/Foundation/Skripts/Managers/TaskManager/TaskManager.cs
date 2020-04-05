using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    [CreateAssetMenu(fileName = "CoroutineManager", menuName = "Managers/CreateCoroutineManager")]
    public class TaskManager : ManagerBase, IAwake {
        
        public static MonoHostComponent host;

        private ITask currentTask;
        private ITask CurrentTask {  get { return currentTask; } }
        private List<ITask> tasks = new List<ITask>();
        private bool isCurrentTask => currentTask != null;
        
        public void OnAwake() {
            if(!host) {
                host = GameObject.Find("[SETUP]").GetComponent<MonoHostComponent>();
            }
        }

#region Перегруженные методы добавления задачи в очередь

        public static void AddTask(IEnumerator _taskAction, Action _callback) {
            var mng = Toolbox.GetManager<TaskManager>();
            if(mng) {
                var newTask = Task.CreateTask(_taskAction, false).Subscrible(_callback);
                mng.AddNewTaskInList(newTask);
            }
        }

        public static void AddTask(IEnumerator _taskAction, bool _highPriority) {
            var mng = Toolbox.GetManager<TaskManager>();
            if(mng) {
                var newTask = Task.CreateTask(_taskAction, _highPriority).Subscrible(null);
                mng.AddNewTaskInList(newTask);
            }
        }

        public static void AddTask(IEnumerator _taskAction, Action _callback, bool _highPriority) {
            var mng = Toolbox.GetManager<TaskManager>();
            if(mng) {
                var newTask = Task.CreateTask(_taskAction, _highPriority).Subscrible(_callback);
                mng.AddNewTaskInList(newTask);
            }
        }

        public static void AddTask(IEnumerator _taskAction) {
            var mng = Toolbox.GetManager<TaskManager>();
            if(mng) {
                var newTask = Task.CreateTask(_taskAction, false).Subscrible(null);
                mng.AddNewTaskInList(newTask);
            }
        }

#endregion
//-------------------------------------------------------------------------------------------------

        private void AddNewTaskInList(ITask _task) {
            if(_task != null) {
                if(_task.HighPriority) {
                    if(isCurrentTask && !currentTask.HighPriority) {
                        currentTask.Stop();
                    }
                    currentTask = _task;
                    currentTask.Subscrible(TaskQueueProcessing).Start();
                }
                tasks.Add(_task);
                if(!isCurrentTask) {
                    currentTask = GetNextTask();
                    if(isCurrentTask) {
                        currentTask.Subscrible(TaskQueueProcessing).Start();
                    }
                }
            }
        }

        private void TaskQueueProcessing() {
            currentTask = GetNextTask();
            if(isCurrentTask) {
                currentTask.Subscrible(TaskQueueProcessing).Start();
            }
        }

        private ITask GetNextTask() {
            if(tasks.Count > 0) {
                var returnValue = tasks[0]; 
                tasks.RemoveAt(0);
                return returnValue;
            } else return null;
        }
    }   
}
