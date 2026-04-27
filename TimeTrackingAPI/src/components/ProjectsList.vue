<template>
  <div>
    <h2>Проекты</h2>
    <button @click="showCreateForm = true">+ Новый проект</button>
    <ul>
      <li v-for="project in projectsStore.projects" :key="project.id">
        {{ project.name }} ({{ project.code }})
        <button @click="editProject(project)">✏️</button>
        <button @click="deleteProject(project.id)">❌</button>
      </li>
    </ul>
    <ProjectForm v-if="showCreateForm" @close="showCreateForm = false" @saved="refresh" />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useProjectsStore } from '@/stores/projects';
import ProjectForm from './ProjectForm.vue';

const projectsStore = useProjectsStore();
const showCreateForm = ref(false);

onMounted(() => projectsStore.fetchProjects());

const deleteProject = async (id) => {
  if (confirm('Удалить проект?')) await projectsStore.deleteProject(id);
};
const refresh = () => projectsStore.fetchProjects();
</script>