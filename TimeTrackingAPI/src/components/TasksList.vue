<template>
  <div>
    <h2>Задачи</h2>
    <div class="controls">
      <select v-model="selectedProjectId">
        <option :value="null">Все проекты</option>
        <option v-for="p in projectsStore.projects" :key="p.Id" :value="p.Id">
          {{ p.Name }}
        </option>
      </select>
      <button @click="showTaskForm = true">+ Новая задача</button>
    </div>

    <div v-if="tasksStore.loading">Загрузка...</div>
    <ul v-else class="tasks-list">
      <li v-for="task in filteredTasks" :key="task.Id" class="task-item">
        <strong>{{ task.Name }}</strong> 
        <span class="project-id">(Проект: {{ task.ProjectId }})</span>
        <span :class="['status', task.IsActive ? 'active' : 'inactive']">
          {{ task.IsActive ? 'Активна' : 'Неактивна' }}
        </span>
        <div class="action-buttons">
          <button @click="editTask(task)" class="edit-btn">✏️</button>
          <button @click="deleteTask(task.Id)" class="delete-btn">❌</button>
        </div>
      </li>
    </ul>

    <TaskForm 
      v-if="showTaskForm || editingTask" 
      :task="editingTask"
      :projectId="selectedProjectId"
      @close="closeForm" 
      @saved="refresh" 
    />
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
const editingTask = ref(null);

onMounted(async () => {
  await projectsStore.fetchProjects();
  await tasksStore.fetchTasks();
});

const filteredTasks = computed(() => {
  if (!selectedProjectId.value) return tasksStore.tasks;
  return tasksStore.tasks.filter(t => t.ProjectId === selectedProjectId.value);
});

const editTask = (task) => {
  editingTask.value = task;
  showTaskForm.value = true;
};

const deleteTask = async (id) => {
  if (confirm('Удалить задачу?')) {
    await tasksStore.deleteTask(id);
    await refresh();
  }
};

const refresh = async () => {
  await tasksStore.fetchTasks();
  closeForm();
};

const closeForm = () => {
  showTaskForm.value = false;
  editingTask.value = null;
};
</script>

<style scoped>
.controls {
  display: flex;
  gap: 1rem;
  margin-bottom: 1rem;
  align-items: center;
}

.tasks-list {
  list-style: none;
  padding: 0;
  margin: 1rem 0;
}

.task-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.5rem 0;
  border-bottom: 1px solid #eee;
  flex-wrap: wrap;
}

.project-id {
  color: #666;
  font-size: 0.9rem;
}

.status {
  padding: 0.2rem 0.6rem;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 500;
}

.status.active {
  background-color: #d4edda;
  color: #155724;
}

.status.inactive {
  background-color: #f8d7da;
  color: #721c24;
}

.action-buttons {
  display: flex;
  gap: 0.5rem;
  margin-left: auto; /* прижимает кнопки вправо, если нужно */
}

button {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.2rem;
  padding: 0.2rem;
  transition: opacity 0.2s;
}

button:hover {
  opacity: 0.7;
}
</style>