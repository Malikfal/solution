<template>
  <div>
    <h2>Задачи</h2>
    <select v-model="selectedProjectId">
      <option :value="null">Все проекты</option>
      <option v-for="p in projects" :value="p.id">{{ p.name }}</option>
    </select>
    <button @click="showTaskForm = true">+ Новая задача</button>
    <ul>
      <li v-for="task in filteredTasks" :key="task.id">
        {{ task.name }} (Проект: {{ task.projectName || task.projectId }})
        <button @click="editTask(task)">✏️</button>
        <button @click="deleteTask(task.id)">❌</button>
      </li>
    </ul>
    <TaskForm :projectId="selectedProjectId" v-if="showTaskForm" @close="showTaskForm = false" @saved="refresh" />
  </div>
</template>

<script setup>


import { ref, computed, onMounted } from 'vue';
import { useTasksStore } from '@/stores/tasks';
import { useProjectsStore } from '@/stores/projects';
import TaskForm from './TaskForm.vue';

const tasksStore = useTasksStore();
const projectsStore = useProjectsStore();
const selectedProjectId = ref(null);
const showTaskForm = ref(false);

const projects = computed(() => projectsStore.projects);
const filteredTasks = computed(() => {
  if (!selectedProjectId.value) return tasksStore.tasks;
  return tasksStore.tasks.filter(t => t.projectId === selectedProjectId.value);
});

onMounted(async () => {
  await projectsStore.fetchProjects();
  await tasksStore.fetchTasks();
});
const refresh = () => tasksStore.fetchTasks();
const deleteTask = async (id) => { /* подтверждение и вызов store */ };
</script>