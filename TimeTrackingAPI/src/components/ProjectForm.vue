<template>
  <div class="modal-overlay">
    <div class="modal-content">
      <h3>{{ isEditing ? 'Редактировать проект' : 'Новый проект' }}</h3>
      
      <div class="form-group">
        <label>Название проекта</label>
        <input v-model="form.name" />
      </div>

      <div class="form-group">
        <label>Код проекта</label>
        <input v-model="form.code" />
      </div>

      <div class="form-group">
        <label><input type="checkbox" v-model="form.isActive" /> Активен</label>
      </div>

      <div class="buttons">
        <button @click="submit">Сохранить</button>
        <button @click="$emit('close')">Отмена</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import api from '@/services/api';

const props = defineProps({ project: Object });
const emit = defineEmits(['close', 'saved']);

const form = ref({
  name: '',
  code: '',
  isActive: true
});
const isEditing = ref(false);

if (props.project) {
  isEditing.value = true;
  form.value = { ...props.project };
}

const submit = async () => {
  try {
    if (isEditing.value) {
      await api.updateProject(props.project.id, form.value);
    } else {
      await api.createProject(form.value);
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