<template>
  <div class="task-manager">
    <h1>Task Manager</h1>
    <div class="add-task">
      <input type="text" v-model="newTaskTitle" placeholder="Add a new task..." @keyup.enter="addTask">
      <button @click="addTask">Add Task</button>
    </div>
    <ul>
      <li v-for="task in tasks" :key="task.id" :class="{ 'completed': task.isCompleted }">
        <input type="checkbox" v-model="task.isCompleted" @change="toggleTask(task)">
                <span>{{ task.title }}</span>
        <button @click="deleteTask(task.id)" class="delete-btn">Delete</button>
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
const errorMessage = ref('');

const fetchTasks = async () => {
  try {
    const response = await axios.get(`${API_BASE}/Task/getalltasks`);
    tasks.value = response.data;
  } catch (error) {
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
      isCompleted: false
    });
    tasks.value.push(response.data);
    newTaskTitle.value = '';
  } catch (error) {
    console.error('Error adding task:', error);
  }
};

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
  margin-bottom: 20px;
}

.add-task input {
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

.error-message {
  color: #fff;
  background: #ff4d4d;
  padding: 10px;
  border-radius: 4px;
  margin-bottom: 15px;
  text-align: center;
}
</style>
