<template>
  <div class="modal-overlay">
    <div class="modal-content">
      <h3>{{ isEditing ? 'Редактировать задачу' : 'Новая задача' }}</h3>
      
      <div class="form-group">
        <label>Название задачи</label>
        <input v-model="form.name" placeholder="Введите название" />
      </div>

      <div class="form-group">
        <label>Проект</label>
        <select v-model="form.projectId">
          <option v-for="p in projects" :key="p.id" :value="p.id">{{ p.name }}</option>
        </select>
      </div>

      <div class="form-group">
        <label>
          <input type="checkbox" v-model="form.isActive" />
          Активна
        </label>
      </div>

      <div class="buttons">
        <button @click="submit">Сохранить</button>
        <button @click="$emit('close')">Отмена</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '@/services/api';
import { useProjectsStore } from '@/stores/projects';

const props = defineProps({ task: Object, projectId: Number });
const emit = defineEmits(['close', 'saved']);

const projectsStore = useProjectsStore();
const projects = ref([]);
const form = ref({
  name: '',
  projectId: null,
  isActive: true
});
const isEditing = ref(false);

onMounted(async () => {
  await projectsStore.fetchProjects();
  projects.value = projectsStore.projects;
  
  if (props.task) {
    isEditing.value = true;
    form.value = { ...props.task };
  } else if (props.projectId) {
    form.value.projectId = props.projectId;
  }
});

const submit = async () => {
  try {
    if (isEditing.value) {
      await api.updateTask(props.task.id, form.value);
    } else {
      await api.createTask(form.value);
    }
    emit('saved');
    emit('close');
  } catch (err) {
    alert(err.response?.data || 'Ошибка');
  }
};
</script>

<style scoped>
.modal-overlay {  }
.modal-content { }
.form-group { margin-bottom: 15px; }
.buttons { display: flex; gap: 10px; justify-content: flex-end; }
</style>