<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-card">
      <div class="modal-header">
        <h3>{{ isEditing ? '✏️ Редактировать проводку' : '⏱️ Новая проводка' }}</h3>
        <button class="close-btn" @click="$emit('close')">✕</button>
      </div>

      <div class="modal-body">
        <!-- Задача -->
        <div class="form-group">
          <label>Задача</label>
          <select v-model="form.TaskId" class="modern-select" required>
            <option :value="null" disabled>— Выберите задачу —</option>
            <option v-for="task in activeTasks" :key="task.Id" :value="task.Id">
              {{ task.Name }} 
              <span v-if="task.ProjectName">({{ task.ProjectName }})</span>
              <span v-else>(Проект: {{ task.ProjectId }})</span>
            </option>
          </select>
        </div>

        <!-- Часы -->
        <div class="form-group">
          <label>Часы (0–24, шаг 0.5)</label>
          <input type="number" step="0.5" v-model.number="form.Hours" min="0" max="24" />
        </div>

        <!-- Описание -->
        <div class="form-group">
          <label>Описание</label>
          <input type="text" v-model="form.Description" placeholder="Что сделано?" />
        </div>

        <!-- Дата -->
        <div class="form-group">
          <label>Дата</label>
          <input type="date" v-model="form.Date" />
        </div>
      </div>

      <div class="modal-footer">
        <button class="btn-cancel" @click="$emit('close')">Отмена</button>
        <button class="btn-save" @click="submit">Сохранить</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '@/services/api';
import { useTasksStore } from '@/stores/tasks';
import { useProjectsStore } from '@/stores/projects';

const props = defineProps({ date: String, entry: Object });
const emit = defineEmits(['close', 'saved']);

const tasksStore = useTasksStore();
const projectsStore = useProjectsStore();
const activeTasks = ref([]);
const form = ref({
  TaskId: null,
  Hours: 8,
  Description: '',
  Date: props.date || new Date().toISOString().slice(0, 10)
});
const isEditing = ref(false);

onMounted(async () => {
  // Загружаем задачи и проекты для отображения имени проекта
  await Promise.all([tasksStore.fetchTasks(), projectsStore.fetchProjects()]);
  
  // Строим массив активных задач с добавлением имени проекта
  const projectsMap = new Map(projectsStore.projects.map(p => [p.Id, p.Name]));
  activeTasks.value = tasksStore.tasks
    .filter(t => t.IsActive === true)
    .map(task => ({
      ...task,
      ProjectName: projectsMap.get(task.ProjectId) || `ID:${task.ProjectId}`
    }));

  if (props.entry) {
    isEditing.value = true;
    form.value = { ...props.entry };
    if (form.value.Date) form.value.Date = form.value.Date.split('T')[0];
  }
});

const submit = async () => {
  // Валидация
  if (!form.value.TaskId) {
    alert('Выберите задачу');
    return;
  }
  if (form.value.Hours <= 0 || form.value.Hours > 24) {
    alert('Часы должны быть в диапазоне от 0 до 24');
    return;
  }
  if (!form.value.Date) {
    alert('Укажите дату');
    return;
  }

  try {
    if (isEditing.value) {
      await api.updateTimeEntry(props.entry.Id, form.value);
    } else {
      await api.createTimeEntry(form.value);
    }
    emit('saved');
    emit('close');
  } catch (err) {
    const msg = err.response?.data || 'Ошибка сохранения проводки';
    alert(msg);
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
  background: rgba(0, 0, 0, 0.6);
  backdrop-filter: blur(4px);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  animation: fadeIn 0.2s ease;
}


.modal-card {
  background: #ffffff;
  border-radius: 20px;
  width: 520px;
  max-width: 90%;
  box-shadow: 0 25px 45px rgba(0, 0, 0, 0.25);
  overflow: hidden;
  animation: slideUp 0.25s ease;
}


.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.2rem 1.5rem;
  background: linear-gradient(135deg, #f5f9ff 0%, #eef2f8 100%);
  border-bottom: 1px solid #e2e8f0;
}
.modal-header h3 {
  margin: 0;
  font-size: 1.35rem;
  font-weight: 600;
  color: #1e2a3a;
}
.close-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #7f8c8d;
  transition: all 0.2s;
  line-height: 1;
  padding: 0 6px;
}
.close-btn:hover {
  color: #e53e3e;
  transform: scale(1.1);
}

/* Тело формы */
.modal-body {
  padding: 1.5rem;
}
.form-group {
  margin-bottom: 1.3rem;
}
.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #2d3748;
  font-size: 0.9rem;
}
.form-group input[type="text"],
.form-group input[type="number"],
.form-group input[type="date"],
.form-group select {
  width: 100%;
  padding: 0.7rem 1rem;
  border: 1px solid #cbd5e0;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.2s;
  box-sizing: border-box;
  background: #fff;
}
.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #4a90e2;
  box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.2);
}


input[type="number"] {
  -moz-appearance: textfield;
}
input[type="number"]::-webkit-inner-spin-button,
input[type="number"]::-webkit-outer-spin-button {
  opacity: 0.5;
}


.modal-footer {
  padding: 1rem 1.5rem 1.5rem;
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  background: #fafcff;
}
.btn-cancel, .btn-save {
  padding: 0.6rem 1.2rem;
  border: none;
  border-radius: 40px;
  font-size: 0.9rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}
.btn-cancel {
  background: #edf2f7;
  color: #4a5568;
}
.btn-cancel:hover {
  background: #e2e8f0;
  transform: translateY(-1px);
}
.btn-save {
  background: linear-gradient(105deg, #2c6e9e, #1e4a76);
  color: white;
  box-shadow: 0 2px 6px rgba(0,0,0,0.1);
}
.btn-save:hover {
  background: linear-gradient(105deg, #1e5a85, #163f60);
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(30, 90, 133, 0.3);
}


@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>