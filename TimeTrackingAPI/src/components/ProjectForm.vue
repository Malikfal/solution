<script setup>
import { ref } from 'vue';
import { useProjectsStore } from '@/stores/projects';

const props = defineProps({ project: Object });
const emit = defineEmits(['close', 'saved']);
const projectsStore = useProjectsStore();

const form = ref({
  Name: '',
  Code: '',
  IsActive: true
});
const isEditing = ref(false);

if (props.project) {
  isEditing.value = true;
  form.value = { ...props.project };
}

const submit = async () => {
  try {
    if (isEditing.value) {
      await projectsStore.updateProject(props.project.Id, form.value);
    } else {
      await projectsStore.addProject(form.value);
    }
    emit('saved');
    emit('close');
  } catch (err) {
    alert(err.response?.data || 'Ошибка');
  }
};
</script>

<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-card">
      <div class="modal-header">
        <h3>{{ isEditing ? '✏️ Редактировать проект' : '✨ Новый проект' }}</h3>
        <button class="close-btn" @click="$emit('close')">✕</button>
      </div>
      
      <div class="modal-body">
        <div class="form-group">
          <label>Название проекта</label>
          <input type="text" v-model="form.Name" placeholder="Введите название" autofocus />
        </div>
        
        <div class="form-group">
          <label>Код проекта</label>
          <input type="text" v-model="form.Code" placeholder="Уникальный код" />
        </div>
        
        <div class="form-group checkbox">
          <label class="checkbox-label">
            <input type="checkbox" v-model="form.IsActive" />
            <span>Активен</span>
          </label>
        </div>
      </div>
      
      <div class="modal-footer">
        <button class="btn-cancel" @click="$emit('close')">Отмена</button>
        <button class="btn-save" @click="submit">Сохранить</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Затемнённый фон */
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

/* Карточка модального окна */
.modal-card {
  background: #ffffff;
  border-radius: 20px;
  width: 460px;
  max-width: 90%;
  box-shadow: 0 25px 45px rgba(0, 0, 0, 0.25);
  overflow: hidden;
  animation: slideUp 0.25s ease;
}

/* Заголовок */
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
  margin-bottom: 1.2rem;
}
.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #2d3748;
  font-size: 0.9rem;
}
.form-group input[type="text"] {
  width: 100%;
  padding: 0.7rem 1rem;
  border: 1px solid #cbd5e0;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.2s;
  box-sizing: border-box;
}
.form-group input[type="text"]:focus {
  outline: none;
  border-color: #4a90e2;
  box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.2);
}
.checkbox-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
}
.checkbox-label input {
  width: 18px;
  height: 18px;
  cursor: pointer;
}
.checkbox-label span {
  font-weight: 500;
  color: #2d3748;
}

/* Нижние кнопки */
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

/* Анимации */
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