from keras.models import load_model


def real(x_test):
    return 0.5 * x_test[0][0] + 0.2 * x_test[0][1] + 0.3 * x_test[0][2] + 0.1 * x_test[0][3] + 0.2 * x_test[0][4]

# Загрузка модели и весов
model = load_model('model.h5')
x_test = [[0.11, 0.3, 2 , 0.1, 3]]

#print(x_test)
# Использование модели для предсказания
predictions = model.predict(x_test)

result = predictions[0][0]
print(predictions)
print(real(x_test))

