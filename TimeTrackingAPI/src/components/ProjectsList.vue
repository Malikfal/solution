<template>
  <div>
    <h2>Проекты</h2>
    <button @click="showCreateForm = true">+ Новый проект</button>
    
    <div v-if="projectsStore.loading">Загрузка...</div>
    <ul v-else class="projects-list">
      <li v-for="project in projectsStore.projects" :key="project.Id" class="project-item">
        <strong>{{ project.Name }}</strong> (код: {{ project.Code }})
        <span :style="{ color: project.IsActive ? 'green' : 'red' }" class="status">
          {{ project.IsActive ? 'Активен' : 'Неактивен' }}
        </span>
        <div class="action-buttons">
          <button @click="editProject(project)" class="edit-btn">✏️</button>
          <button @click="deleteProject(project.Id)" class="delete-btn">❌</button>
        </div>
      </li>
    </ul>

    <ProjectForm 
      v-if="showCreateForm || editingProject" 
      :project="editingProject"
      @close="closeForm" 
      @saved="refresh" 
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useProjectsStore } from '@/stores/projects';
import ProjectForm from './ProjectForm.vue';

const projectsStore = useProjectsStore();
const showCreateForm = ref(false);
const editingProject = ref(null);

onMounted(async () => {
  await projectsStore.fetchProjects();
});

const editProject = (project) => {
  editingProject.value = project;
  showCreateForm.value = true;
};

const deleteProject = async (id) => {
  if (confirm('Удалить проект?')) {
    await projectsStore.deleteProject(id);
  }
};

const refresh = async () => {
  await projectsStore.fetchProjects();
  closeForm();
};

const closeForm = () => {
  showCreateForm.value = false;
  editingProject.value = null;
};
</script>

<style scoped>
.projects-list {
  list-style: none;
  padding: 0;
  margin: 1rem 0;
}

.project-item {
  display: flex;
  align-items: center;
  gap: 1rem;               /* расстояние между названием, кодом, статусом и кнопками */
  padding: 0.5rem 0;
  border-bottom: 1px solid #eee;
}

.status {
  font-weight: 500;
}

.action-buttons {
  display: flex;
  gap: 0.5rem;             /* расстояние между кнопками ✏️ и ❌ */
}

button {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.2rem;
  padding: 0.2rem;
}
button:hover {
  opacity: 0.7;
}
</style>