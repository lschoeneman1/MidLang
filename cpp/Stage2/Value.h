#ifndef VALUE_H
#define VALUE_H

#include <string>

/**
 * Value - Represents a value that can be either an integer or a string.
 * 
 * This is a simple class that junior students can easily understand.
 * It uses a type field to track what kind of value it holds.
 */
class Value {
public:
    enum Type {
        INT,
        STRING
    };

    Type type;
    int intValue;
    std::string stringValue;

    // Constructor for integer values
    Value(int val) : type(INT), intValue(val), stringValue("") {}

    // Constructor for string values
    Value(const std::string& val) : type(STRING), intValue(0), stringValue(val) {}

    // Convert to string (for printing and concatenation)
    std::string toString() const {
        if (type == INT) {
            return std::to_string(intValue);
        } else {
            return stringValue;
        }
    }

    // Check if value is an integer
    bool isInt() const {
        return type == INT;
    }

    // Check if value is a string
    bool isString() const {
        return type == STRING;
    }

    // Get integer value (throws if not an integer)
    int getInt() const {
        if (type != INT) {
            throw std::runtime_error("Value is not an integer");
        }
        return intValue;
    }

    // Get string value
    std::string getString() const {
        return toString();
    }
};

#endif // VALUE_H

