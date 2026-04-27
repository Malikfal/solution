<template>
  <div class="modal-overlay">
    <div class="modal-content">
      <h3>{{ isEditing ? 'Редактировать проводку' : 'Новая проводка' }}</h3>
      
      <div class="form-group">
        <label>Задача *</label>
        <select v-model="form.taskId" required>
          <option v-for="task in activeTasks" :key="task.id" :value="task.id">
            {{ task.name }} ({{ task.projectName || task.projectId }})
          </option>
        </select>
      </div>

      <div class="form-group">
        <label>Часы (0–24)</label>
        <input type="number" step="0.5" v-model="form.hours" min="0.01" max="24" />
      </div>

      <div class="form-group">
        <label>Описание</label>
        <input v-model="form.description" placeholder="Что сделано?" />
      </div>

      <div class="form-group">
        <label>Дата</label>
        <input type="date" v-model="form.date" />
      </div>

      <div class="buttons">
        <button @click="submit" class="save-btn">Сохранить</button>
        <button @click="$emit('close')" class="cancel-btn">Отмена</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '@/services/api';

const props = defineProps({
  date: String,
  entry: Object
});
const emit = defineEmits(['close', 'saved']);

const form = ref({
  taskId: null,
  hours: 8,
  description: '',
  date: props.date || new Date().toISOString().slice(0, 10)
});
const activeTasks = ref([]);
const isEditing = ref(false);

onMounted(async () => {
  // Загружаем только активные задачи
  const res = await api.getTasks();
  activeTasks.value = res.data.filter(t => t.isActive);
  
  if (props.entry) {
    isEditing.value = true;
    form.value = { ...props.entry };
    // Форматируем дату для input type="date"
    if (form.value.date) {
      form.value.date = form.value.date.split('T')[0];
    }
  }
});

const submit = async () => {
  try {
    if (isEditing.value) {
      await api.updateTimeEntry(props.entry.id, form.value);
    } else {
      await api.createTimeEntry(form.value);
    }
    emit('saved');
    emit('close');
  } catch (err) {
    alert(err.response?.data || 'Ошибка сохранения');
  }
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}
.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 400px;
  max-width: 90%;
}
.form-group {
  margin-bottom: 15px;
}
.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}
.form-group select, .form-group input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}
.save-btn {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}
.cancel-btn {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}
</style>