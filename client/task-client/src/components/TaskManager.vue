<template>
  <div class="task-manager">
    <h1>Task Manager</h1>
    <div class="add-task">
     <input type="text" v-model="newTaskTitle" placeholder="Add a new task..." @keyup.enter="addTask"
        :disabled="isTaskFetchFailed">
            <label class="due-date-label" for="due-date-input">Due Date:</label>
      <input id="due-date-input" type="date" v-model="newTaskDueDate" :disabled="isTaskFetchFailed" />
      <button @click="addTask" :disabled="isTaskFetchFailed">Add Task</button>
    </div>
    <ul>
      <li v-for="task in sortedTasks" :key="task.id" :class="{ 'completed': task.isCompleted }">
        <input type="checkbox" v-model="task.isCompleted" @change="toggleTask(task)" :disabled="isTaskFetchFailed">
        <span>{{ task.title }}</span>
        <span class="due-date">Due: {{ formatDate(task.dueDate) }}</span>
        <button @click="deleteTask(task.id)" class="delete-btn" :disabled="isTaskFetchFailed">Delete</button>
      </li>
    </ul>
    <div v-if="errorMessage" class="error-message">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script setup>

import { ref, onMounted } from 'vue';
import axios from 'axios';
import { API_BASE } from '../../config';

const tasks = ref([]);
const newTaskTitle = ref('');
const newTaskDueDate = ref(new Date().toISOString().slice(0, 10));
const errorMessage = ref('');
const isTaskFetchFailed = ref(false);

const fetchTasks = async () => {
  isTaskFetchFailed.value = false;
  try {
    const response = await axios.get(`${API_BASE}/Task/getalltasks`);
    // Sort by due date ascending
    tasks.value = response.data.sort((a, b) => new Date(a.dueDate) - new Date(b.dueDate));
  } catch (error) {
    isTaskFetchFailed.value = true;
    errorMessage.value = 'Failed to get tasks from the server.';
    console.error('Error fetching tasks:', error);
  }
};

const addTask = async () => {
  if (newTaskTitle.value.trim() === '') {
    return;
  }
  try {
    const response = await axios.post(`${API_BASE}/Task/createtask`, {
      title: newTaskTitle.value,
      isCompleted: false,
      dueDate: newTaskDueDate.value
    });
    tasks.value.push(response.data);
    // Sort after adding
    tasks.value.sort((a, b) => new Date(a.dueDate) - new Date(b.dueDate));
    newTaskTitle.value = '';
    newTaskDueDate.value = new Date().toISOString().slice(0, 10);
  } catch (error) {
    console.error('Error adding task:', error);
  }
};
// Computed sorted tasks for rendering
import { computed } from 'vue';
const sortedTasks = computed(() => {
  return [...tasks.value].sort((a, b) => new Date(a.dueDate) - new Date(b.dueDate));
});

function formatDate(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  return d.toLocaleDateString();
}

const toggleTask = async (task) => {
  try {
    await axios.patch(`${API_BASE}/Task/updatetaskstatus/${task.id}`, {
      isCompleted: task.isCompleted
    });
  } catch (error) {
    console.error('Error updating task status:', error);
    // Optionally revert the change in UI if backend fails
    task.isCompleted = !task.isCompleted;
  }
};

const deleteTask = async (id) => {
  errorMessage.value = '';
  try {
    await axios.delete(`${API_BASE}/Task/deletetask/${id}`);
    tasks.value = tasks.value.filter(task => task.id !== id);
  } catch (error) {
    if (error.response && error.response.status === 404) {
      errorMessage.value = 'Task not found. It may have already been deleted.';
    } else {
      errorMessage.value = 'Error deleting task.';
    }
    console.error('Error deleting task:', error);
  }
};

onMounted(fetchTasks);
</script>

<style scoped>
.task-manager {
  max-width: 600px;
  margin: 0 auto;
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
}

.add-task {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.add-task label,
.add-task input[type="date"] {
  margin-right: 10px;
}

.add-task input[type="text"] {
  flex-grow: 1;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.add-task button {
  padding: 10px 15px;
  border: none;
  background-color: #42b983;
  color: white;
  border-radius: 4px;
  margin-left: 10px;
  cursor: pointer;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: flex;
  align-items: center;
  padding: 10px 0;
  border-bottom: 1px solid #eee;
}

li.completed span {
  text-decoration: line-through;
  color: #999;
}

li input[type="checkbox"] {
  margin-right: 10px;
}

.delete-btn {
  margin-left: auto;
  padding: 5px 10px;
  border: none;
  background-color: #ff4d4d;
  color: white;
  border-radius: 4px;
  cursor: pointer;
}
.due-date {
  margin-left: 15px;
  color: #888;
  font-size: 0.95em;
}
.due-date-label {
  margin-left: 10px;
  margin-right: 5px;
  font-weight: 500;
}

.error-message {
  color: #fff;
  background: #ff4d4d;
  padding: 10px;
  border-radius: 4px;
  margin-bottom: 15px;
  text-align: center;
}
</style>
